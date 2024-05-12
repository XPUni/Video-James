using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDeleter : MonoBehaviour
{  
    private Rigidbody2D rb;
    
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
        if (collision.gameObject.tag == "Player" && gameObject.tag == "Finish")
        {
            Destroy(gameObject);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerHand" && gameObject.tag != "DogNose")
        {
            if(gameObject.tag == "earPod"){
                collision.gameObject.transform.parent.gameObject.GetComponent<PlayerController>().hasEarPod = true;
                collision.gameObject.transform.parent.gameObject.GetComponent<PlayerController>().mainMusic.mute = false;
                //Debug.Log(hasEarPod);
            }
            if(gameObject.tag == "SecondEar" && collision.gameObject.transform.parent.gameObject.GetComponent<PlayerController>().hasEarPod == false){  
                //Debug.Log(hasEarPod);
                Debug.Log("You don't have the second earpod.");
                return;
            }
            //player.GetComponent<PlayerController>().key1 = true;    
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Player" && gameObject.tag == "DogNose")
        {
            if (gameObject.transform.childCount != 0)
            {
                Destroy(gameObject.transform.GetChild(0).gameObject);
            }
        }
    }
}
