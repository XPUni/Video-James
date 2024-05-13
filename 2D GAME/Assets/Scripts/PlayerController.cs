using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public AudioSource mainMusic;
    public AudioSource earPodMusic;
    public GameObject levelExitClose;
    public GameObject levelExitOpen;
    public Animator animator;
    public List<char> keys = new List<char>();
    public bool grounded = true;
    public bool hasEarPod = false;
    private Rigidbody2D rb;
    public bool hasFlower = false;
    private GameObject light;

    private float horizontal_top_speed;
    private float horizontal_target_speed;

    private float coyoteTime = 0.2f;
    private float coyoteTracker;

    private float bufferTime = 0.2f;
    private float bufferTracker;

    private static bool triggerThisFrame = false;
    private static bool openedDoorThisFrame = false;

    private bool onLadder = false;

    public KeyTracker key_tracker;
    public bool hasArm = false;
    private bool tiny;
    public float tinyTimer = 6f;
    public float tinyTime = 0f;
    public GameObject cake;
    private Dictionary<GameObject, float> cakes = new Dictionary<GameObject, float>();
    private bool hasNose;

    // Start is called before the first frame update
    void Start()
    {
        hasNose = false;
        if(earPodMusic){
            earPodMusic.mute = true;
        }
        
        rb = GetComponent<Rigidbody2D>();
        //levelExitOpen.SetActive(false);
        if(gameObject.transform.childCount>=8){
            if(gameObject.transform.GetChild(7).gameObject.name == "playerLight"){
                light = gameObject.transform.GetChild(7).gameObject;
                light.SetActive(false);
            }
        }
        
        Scene scene = SceneManager.GetActiveScene();
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
        gameObject.transform.GetChild(3).gameObject.SetActive(false);
        gameObject.transform.GetChild(4).gameObject.SetActive(false);
        gameObject.transform.GetChild(6).gameObject.SetActive(false);

        if(scene.buildIndex==6){
            mainMusic.mute = true;

        }
        else{
            mainMusic.mute = false;
        }
        Debug.Log(scene.buildIndex);
        if (scene.buildIndex > 2)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        if (scene.buildIndex > 4)
        {
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }
        if (scene.buildIndex > 6)
        {
            gameObject.transform.GetChild(2).gameObject.SetActive(true);
        }
        if (scene.buildIndex > 8)
        {
            gameObject.transform.GetChild(3).gameObject.SetActive(true);
            //earPodMusic.mute = false;
        }
        //cakes = new Dictionary<GameObject, float>();
        Debug.Log(cakes);
    }

    public bool IsGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y - 0.6f), 0.45f); // Adjust radius as needed
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag == "Ground" || collider.gameObject.tag.StartsWith("Door"))
            {
                return true;
            }
        }
        return false;
    }
    public bool CanExpand()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y + transform.localScale.y * 0.5f), new Vector2(transform.localScale.x * 2, transform.localScale.y * 2), 0f); // Adjust radius as needed
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag == "Ground" || collider.gameObject.tag.StartsWith("Door"))
            {
                return false;
            }
        }
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        triggerThisFrame = false;
        openedDoorThisFrame = false;
        //added
        
        if (cakes.Count > 0)
        {
            Tiny();
            tiny = true;
            gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            CapsuleCollider2D collider = gameObject.GetComponent<CapsuleCollider2D>();
            collider.size = new Vector2(0.5f, 1);
        }
        else if (CanExpand())
        {
            tiny = false;
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            CapsuleCollider2D collider = gameObject.GetComponent<CapsuleCollider2D>();
            collider.size = new Vector2(1f, 2f);
        }
        //if (hasArm)
        //{
          //  gameObject.transform.GetChild(0).gameObject.SetActive(true);
        //}
        //else
        //{
          //  gameObject.transform.GetChild(0).gameObject.SetActive(true);
        //}

        //^^
        if (Input.GetKeyDown(KeyCode.R)) { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
        grounded = IsGrounded();
        animator.SetBool("isJump", !grounded);
        // coyote time and jump buffering
        if (grounded) { coyoteTracker = coyoteTime; } else { coyoteTracker -= Time.deltaTime; }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) { bufferTracker = bufferTime; } else { bufferTracker -= Time.deltaTime; }
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
        {
            if (rb.velocity.y > 0) { rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f); }
        }
        DecideGravity();
        Walk();
        if (bufferTracker > 0f && (coyoteTracker > 0f)) { Jump(); }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "LevelExit" && !levelExitClose.activeSelf){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
        if(collision.gameObject.tag == "Mirror" && hasNose){
            SceneManager.LoadScene(0);
        }
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Finish")
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            levelExitClose.SetActive(false);
            //animator.SetTrigger("gottenArm");
        }
        if (collision.gameObject.tag == "Nose"&& hasFlower)
        {
            Destroy(collision.gameObject);
            gameObject.transform.GetChild(4).gameObject.SetActive(true);
            hasNose = true;
        }
        if(collision.gameObject.tag == "Mouth"){
            gameObject.transform.GetChild(3).gameObject.SetActive(true);
            levelExitClose.SetActive(false);
        }
        if (collision.gameObject.tag == "GetNose")
        {
            gameObject.transform.GetChild(4).gameObject.SetActive(true);
        }
        if (collision.gameObject.tag.StartsWith("Door") && keys.Contains(collision.gameObject.tag[collision.gameObject.tag.Length - 1]))
        {
            if (!openedDoorThisFrame)
            {
                openedDoorThisFrame = true;
                keys.Remove(collision.gameObject.tag[collision.gameObject.tag.Length - 1]);
                Destroy(collision.gameObject);
                if (key_tracker != null)
                {
                    key_tracker.DrawKeys();
                }
            }
            //keys.Remove(collision.gameObject.tag[collision.gameObject.tag.Length - 1]);
            //Destroy(collision.gameObject);
        }
        if(collision.gameObject.name == "Cake")
        {
            cakes[collision.gameObject] = tinyTimer;
            collision.gameObject.SetActive(false);
            tiny = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "firstEar"){
            earPodMusic.mute = false;
            Debug.Log(earPodMusic.mute);

        }
        //Debug.Log(collision.gameObject.name);
        if (triggerThisFrame) { return; }
        triggerThisFrame = true;
        if (collision.gameObject.tag == "DogNose")
        {
            gameObject.transform.GetChild(6).gameObject.SetActive(true);
        }
        if (collision.gameObject.tag == "DogNoseRemove")
        {
            gameObject.transform.GetChild(6).gameObject.SetActive(false);
            collision.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        if(collision.gameObject.tag == "Flower")
        {
            hasFlower = true;
            collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.tag == "Ladder" && gameObject.transform.GetChild(0).gameObject.activeSelf)
        {
            float ladderTop = collision.gameObject.GetComponent<BoxCollider2D>().size.y / 2 + collision.gameObject.transform.position.y;
            if (grounded || transform.position.y > ladderTop)
            {
                onLadder = true;
            }
        }
        if (collision.gameObject.tag == "Stinky" && (gameObject.transform.GetChild(6).gameObject.activeSelf || gameObject.transform.GetChild(4).gameObject.activeSelf))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (collision.gameObject.tag.StartsWith("Key") && gameObject.transform.GetChild(0).gameObject.activeSelf)
        {
            keys.Add(collision.gameObject.tag[collision.gameObject.tag.Length - 1]);
            Destroy(collision.gameObject);
            if (key_tracker != null)
            {
                key_tracker.DrawKeys();
            }
        }
        if (collision.gameObject.tag == "EndOfLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if ((collision.gameObject.tag == "SecondEar" && hasEarPod)) {
            gameObject.transform.GetChild(2).gameObject.SetActive(true);
            levelExitClose.SetActive(false);
                
        }
        if(collision.gameObject.tag=="enterDarkness"){
            Debug.Log("I entered darkness");
            light.SetActive(true);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder" && gameObject.transform.GetChild(0).gameObject.activeSelf)
        {
            float ladderTop = collision.gameObject.GetComponent<BoxCollider2D>().size.y / 2 + collision.gameObject.transform.position.y;
            if (grounded || transform.position.y > ladderTop)
            {
                onLadder = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Ladder" && gameObject.transform.GetChild(0).gameObject.activeSelf) {
            onLadder = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 3);
        }
        if(collision.gameObject.tag=="enterDarkness"){
            Debug.Log("I exited darkness");
            light.SetActive(false);
        }
    }
    void DecideGravity() {
            if (onLadder) {
                rb.gravityScale = 0f;
                rb.velocity = new Vector2(rb.velocity.x, 10 * Input.GetAxis("Vertical"));
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
        if (horizontal_target_speed != 0)
        {
            animator.SetBool("isMove", true);
        }
        else { animator.SetBool("isMove", false); }
        rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, horizontal_target_speed, 50f * Time.deltaTime), rb.velocity.y);
        //if (rb.velocity.x < 0) { gameObject.transform.localScale = new Vector3(-1,1,1); }
        //else if (rb.velocity.x > 0) { gameObject.transform.localScale = new Vector3(1,1,1); }
        if (tiny)
        {
            if (rb.velocity.x < 0) { gameObject.transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f); }
            else if (rb.velocity.x > 0) { gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); }
        }
        else
        {
            if (rb.velocity.x < 0) { gameObject.transform.localScale = new Vector3(-1, 1, 1); }
            else if (rb.velocity.x > 0) { gameObject.transform.localScale = new Vector3(1, 1, 1); }
        }
    }
    void Jump()
    {
        animator.SetBool("isJump", true);
        bufferTracker = 0f;
        rb.velocity = new Vector2(rb.velocity.x, 10f);
        grounded = false;
        coyoteTracker = 0f;
    }

    void Tiny()
    {
        Dictionary<GameObject, float>.KeyCollection cakesColl = cakes.Keys;
        List<GameObject> cakesList = new List<GameObject>();
        foreach (GameObject cake in cakesColl)
        {
            cakesList.Add(cake);
        }
        foreach (GameObject cake in cakesList)
        {
            if (cakes[cake] > 0f) { cakes[cake] -= Time.deltaTime; }
            else
            {
                cakes.Remove(cake);
                cake.SetActive(true);
            }
        }
        
    }
}