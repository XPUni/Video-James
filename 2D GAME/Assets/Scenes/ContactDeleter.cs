using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDeleter : MonoBehaviour
{
    private Rigidbody2D rb;
    public SpriteRenderer renderer;
    public Sprite mysprite;
    public GameObject player;
    public BoxCollider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Player" && gameObject.tag == "Door1")
        {
            // key1 = true;
            bool keybool = player.GetComponent<PlayerController>().key1;
            if (keybool==true)
            {
                renderer.sprite = mysprite;
                //Physics2D.IgnoreCollision(collider, collision.collider);
                Destroy(gameObject);
            }
            
        }
        if (collision.gameObject.tag == "Player" && gameObject.tag == "Finish")
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerHand" && gameObject.tag == "Key1")
        {
            player.GetComponent<PlayerController>().key1 = true;    
            Destroy(gameObject);
        }
    }
}
