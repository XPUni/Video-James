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

    public TextMeshProUGUI name;
    public string speakerName;

    public GameObject handicon;
    public GameObject icon;


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
                name.text = speakerName;
                Destroy(gameObject);
                if(speakerName == "You")
                {
                    icon.SetActive(true);
                    handicon.SetActive(false);
                }
                else if (speakerName == "Hands")
                {
                    handicon.SetActive(true);
                    icon.SetActive(false);
                }
                else
                {
                    handicon.SetActive(true);
                    icon.SetActive(false);
                }
            }
        }
    } 
}
