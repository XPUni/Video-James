using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDeleter : MonoBehaviour
{  
    private bool hasEarPod;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hasEarPod = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && gameObject.tag == "Finish")
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerHand")
        {
            if(gameObject.tag == "earPod"){
                hasEarPod = true;
                Debug.Log(hasEarPod);
            }
            if(gameObject.tag == "SecondEar" && !hasEarPod){  
                Debug.Log("You don't have the second earpod.");
                return;
            }
            //player.GetComponent<PlayerController>().key1 = true;    
            Destroy(gameObject);
        }
    }
}
