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
    private Rigidbody2D rb;

    private float horizontal_top_speed;
    private float horizontal_target_speed;

    private float coyoteTime = 0.2f;
    private float coyoteTracker;

    private float bufferTime = 0.2f;
    private float bufferTracker;

    private static bool triggerThisFrame = false;

    private bool onLadder = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //gameObject.transform.GetChild(0).gameObject.SetActive(false);
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

    void DrawKeys() {
        GameObject canvas = GameObject.Find("Canvas");

        foreach (Transform child in canvas.transform) {
            Debug.Log(child.name);
            if (child.name.StartsWith("key")) {
                Destroy(child);
            }
        }
        float xpos = 0;
        foreach (char k in keys) {
            GameObject NewObj = new GameObject(); //Create the GameObject
            Image NewImage = NewObj.AddComponent<Image>(); //Add the Image Component script
            NewImage.sprite = currentSprite; //Set the Sprite of the Image Component on the new GameObject
            NewObj.GetComponent<RectTransform>().SetParent(ParentPanel.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel.
            NewObj.SetActive(true); //Activate the GameObject
        }
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
        }
        if(collision.gameObject.tag.StartsWith("Door") && keys.Contains(collision.gameObject.tag[collision.gameObject.tag.Length - 1]))
        {
            keys.Remove(collision.gameObject.tag[collision.gameObject.tag.Length - 1]);
            Destroy(collision.gameObject);
            DrawKeys();
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
            DrawKeys();
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
        bufferTracker = 0f;
        rb.velocity = new Vector2(rb.velocity.x, 10f);
        grounded = false;
    }
}
