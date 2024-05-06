using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyTracker : MonoBehaviour
{
    public GameObject player;
    public Image key1;
    public Image key2;
    public Image key3;
    public List<GameObject> ifkeys = new List<GameObject>();
    public void DrawKeys() {
        Image new_key;
        new_key = Instantiate(key1, transform);
        float xpos = 10;
        foreach (Transform child in transform) {
            if (child.gameObject.name.EndsWith("(Clone)")) {
                Destroy(child.gameObject);
            }
        }
        foreach (GameObject g in ifkeys) {
            g.SetActive(player.GetComponent<PlayerController>().keys.Count > 0);
        }
        foreach (char k in player.GetComponent<PlayerController>().keys) {
            if (k=='1') {
                new_key = Instantiate(key1, transform);
            } else if (k=='2') {
                new_key = Instantiate(key2, transform);
            } else if (k=='3') {
                new_key = Instantiate(key3, transform);
            }
            new_key.rectTransform.offsetMin = new Vector3(xpos, -60, 0);
            new_key.rectTransform.offsetMax = new Vector3(xpos+25, -35, 0);
            xpos += 30; 
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        player.GetComponent<PlayerController>().key_tracker = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
