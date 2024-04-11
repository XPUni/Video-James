using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool key1 = false;
    public bool grounded = false;
    public bool dubjump = true;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
       
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
        Jump();
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        if(collision.gameObject.tag == "Key1" && gameObject.transform.GetChild(0).gameObject.activeSelf)
        {
            key1 = true;
        }
        if(collision.gameObject.tag == "Door1" && key1 == true)
        {
            //key1 = false;
        }
    }
        void Walk()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalMovement * 9, rb.velocity.y);
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            if(grounded)
        {
            rb.AddForce(new Vector2(0, 10f), ForceMode2D.Impulse);
                dubjump = true;
                grounded = false;
        }
        else if (dubjump)
            {
                rb.AddForce(new Vector2(0, 10f), ForceMode2D.Impulse);
                dubjump = false;
            }
    }
}
