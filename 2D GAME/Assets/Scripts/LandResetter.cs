using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LandResetter : MonoBehaviour
{
    public GameObject player;
    private TilemapCollider2D rb;
    // Start is called before the first frame update
    void Start()
    {rb = GetComponent<TilemapCollider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Debug.LogWarning("hit f=g=orubd");
            player.GetComponent<PlayerController>().grounded = true;
            //player.GetComponent<PlayerController>().dubjump = true;
            //Destroy(gameObject);
        }
    }
}
