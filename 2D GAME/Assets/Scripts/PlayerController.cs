using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //animator. For animation purposes
    public Animator animator;

    public List<char> keys = new List<char>();
    public bool grounded = false;
    public bool dubjump = true;
    private Rigidbody2D rb;

    private float horizontal_top_speed;
    private float horizontal_target_speed;

    private float coyoteTime = 0.2f;
    private float coyoteTracker;

    private float bufferTime = 0.2f;
    private float bufferTracker;

    private bool onLadder = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {SceneManager.LoadScene(SceneManager.GetActiveScene().name);}
        // coyote time and jump buffering
        if (grounded) {coyoteTracker = coyoteTime;} else { coyoteTracker -= Time.deltaTime; }
        if (Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.UpArrow)||Input.GetKeyDown(KeyCode.W)) { bufferTracker = bufferTime; } else { bufferTracker -= Time.deltaTime; }
        if (Input.GetKeyUp(KeyCode.Space)||Input.GetKeyUp(KeyCode.UpArrow)||Input.GetKeyUp(KeyCode.W))
        { 
            if (rb.velocity.y > 0) { rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y*0.5f); }
        }
        DecideGravity();
        Walk();
        if (bufferTracker > 0f && (coyoteTracker > 0f || dubjump)) { Jump(); }
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Finish")
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        if(collision.gameObject.tag.StartsWith("Door") && keys.Contains(collision.gameObject.tag[collision.gameObject.tag.Length - 1]))
        {
            keys.Remove(collision.gameObject.tag[collision.gameObject.tag.Length - 1]);
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Ladder") {
            onLadder = true;
        }
        if (collision.gameObject.tag.StartsWith("Key") && gameObject.transform.GetChild(0).gameObject.activeSelf)
        {
            keys.Add(collision.gameObject.tag[collision.gameObject.tag.Length - 1]);
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Ladder") {
            onLadder = false;
            rb.velocity = new Vector2(rb.velocity.x, 0);
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
        else{
            animator.SetBool("isMove",false);
        }
        rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, horizontal_target_speed, 50f*Time.deltaTime), rb.velocity.y);
        if (rb.velocity.x < 0) { gameObject.transform.localScale = new Vector3(-1,1,1); }
        else if (rb.velocity.x > 0) { gameObject.transform.localScale = new Vector3(1,1,1); }
    }
    void Jump()
    {
        bufferTracker = 0f;
        rb.velocity = new Vector2(rb.velocity.x, 10f);
        if (grounded) { grounded = false; } else { dubjump = false; }
    }
}
