using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmelllShower : MonoBehaviour
{
    public GameObject ToExist;
    public int counter;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject.transform.GetChild(6)!= null)
        {
            ToExist.transform.GetChild(counter).gameObject.SetActive(true);
                Destroy(gameObject);
        }
    }
}
