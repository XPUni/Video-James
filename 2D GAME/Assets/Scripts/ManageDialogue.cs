using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManageDialogue : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public string new_text;
    public GameObject existenceCheck;
    public bool exists;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            if ((existenceCheck != null) == exists) {
                textMeshPro.text = new_text;
                Destroy(gameObject);
            }
        }
    } 
}
