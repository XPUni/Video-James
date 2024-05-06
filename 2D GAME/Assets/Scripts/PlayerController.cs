using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public List<char> keys = new List<char>();
    public bool grounded = true;
    public bool hasEarPod = false;
    private Rigidbody2D rb;

    private float horizontal_top_speed;
    private float horizontal_target_speed;

    private float coyoteTime = 0.2f;
    private float coyoteTracker;

    private float bufferTime = 0.2f;
    private float bufferTracker;

    private static bool triggerThisFrame = false;

    private bool onLadder = false;

    public KeyTracker key_tracker;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Scene scene = SceneManager.GetActiveScene();
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
        gameObject.transform.GetChild(3).gameObject.SetActive(false);
        gameObject.transform.GetChild(4).gameObject.SetActive(false);
        if(scene.buildIndex>0){
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        if(scene.buildIndex>1){
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }
        if(scene.buildIndex>2){
            gameObject.transform.GetChild(2).gameObject.SetActive(true);
        }
        if(scene.buildIndex>3){
            gameObject.transform.GetChild(3).gameObject.SetActive(true);
        }
        
    }

    public bool IsGrounded() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y-0.6f), 0.45f); // Adjust radius as needed
        foreach (Collider2D collider in colliders) {
            if (collider.gameObject.tag == "Ground" || collider.gameObject.tag.StartsWith("Door")) {
                return true;
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        triggerThisFrame = false;
        if (Input.GetKeyDown(KeyCode.R)) {SceneManager.LoadScene(SceneManager.GetActiveScene().name);}
        grounded = IsGrounded();
        animator.SetBool("isJump",!grounded);
        // coyote time and jump buffering
        if (grounded) {coyoteTracker = coyoteTime;} else { coyoteTracker -= Time.deltaTime; }
        if (Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.UpArrow)||Input.GetKeyDown(KeyCode.W)) { bufferTracker = bufferTime; } else { bufferTracker -= Time.deltaTime; }
        if (Input.GetKeyUp(KeyCode.Space)||Input.GetKeyUp(KeyCode.UpArrow)||Input.GetKeyUp(KeyCode.W))
        { 
            if (rb.velocity.y > 0) { rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y*0.5f); }
        }
        DecideGravity();
        Walk();
        if (bufferTracker > 0f && (coyoteTracker > 0f)) { Jump(); }
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Finish")
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            //animator.SetTrigger("gottenArm");
        }
        if(collision.gameObject.tag == "SecondEar"){
            gameObject.transform.GetChild(2).gameObject.SetActive(true);
        }
        if(collision.gameObject.tag.StartsWith("Door") && keys.Contains(collision.gameObject.tag[collision.gameObject.tag.Length - 1]))
        {
            keys.Remove(collision.gameObject.tag[collision.gameObject.tag.Length - 1]);
            Destroy(collision.gameObject);
            key_tracker.DrawKeys();
        }
        if(collision.gameObject.tag == "LevelExit"){
            if(gameObject.transform.GetChild(2).gameObject.activeSelf   ){
                Destroy(collision.gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        //Debug.Log(collision.gameObject.name);
        if (triggerThisFrame) { return; }
        triggerThisFrame = true;

        if (collision.gameObject.tag == "Ladder" && gameObject.transform.GetChild(0).gameObject.activeSelf) {
            float ladderTop = collision.gameObject.GetComponent<BoxCollider2D>().size.y/2+collision.gameObject.transform.position.y;
            if (grounded || transform.position.y > ladderTop) {
                onLadder = true;
            }
        }
        if (collision.gameObject.tag.StartsWith("Key") && gameObject.transform.GetChild(0).gameObject.activeSelf)
        {
            keys.Add(collision.gameObject.tag[collision.gameObject.tag.Length - 1]);
            Destroy(collision.gameObject);
            key_tracker.DrawKeys();
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Ladder"  && gameObject.transform.GetChild(0).gameObject.activeSelf) {
            onLadder = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y/3);
        }
    }
    void DecideGravity() {
        if (onLadder) {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, 10*Input.GetAxis("Vertical"));
        } else if (rb.velocity.y < -15f) {
            rb.velocity = new Vector2(rb.velocity.x, -15);
        } else if (rb.velocity.y >= -20f && rb.velocity.y < -1f) {
            rb.gravityScale = 4f;
        } else if (rb.velocity.y >= -1f && rb.velocity.y < 0.5f) {
            rb.gravityScale = 1f; // make the player more floaty at the peak of their jump
        } else if (rb.velocity.y >= 0.5f) {
            rb.gravityScale = 1.6f;
        }
    }

    void Walk()
    {
        if (grounded) { horizontal_top_speed = 8f; }
        else { horizontal_top_speed = 5f; }
        
        horizontal_target_speed = horizontal_top_speed * Input.GetAxisRaw("Horizontal");
        if(horizontal_target_speed!=0){
            animator.SetBool("isMove",true);
        }
        else{animator.SetBool("isMove", false);}
        rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, horizontal_target_speed, 50f*Time.deltaTime), rb.velocity.y);
        if (rb.velocity.x < 0) { gameObject.transform.localScale = new Vector3(-1,1,1); }
        else if (rb.velocity.x > 0) { gameObject.transform.localScale = new Vector3(1,1,1); }
    }
    void Jump()
    {
        animator.SetBool("isJump", true);
        bufferTracker = 0f;
        rb.velocity = new Vector2(rb.velocity.x, 10f);
        grounded = false;
    }
}
