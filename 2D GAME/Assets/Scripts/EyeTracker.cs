using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class EyeTracker : MonoBehaviour
{
    UnityEngine.Rendering.Universal.ColorAdjustments vignette;
    private int eyes;
    private bool triggerThisFrame = false;
    void Start()
        {
            eyes = 0;
            UnityEngine.Rendering.VolumeProfile volumeProfile = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
            if(!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
            
            // You can leave this variable out of your function, so you can reuse it throughout your class.
            
            if(!volumeProfile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));
            
            vignette.saturation.Override(-100f);
        }

    void Update() {
        triggerThisFrame = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (triggerThisFrame) { return; }
        triggerThisFrame = true;
        if (collision.gameObject.tag == "Eye") {
            eyes += 1;
            Destroy(collision.gameObject);
            if (eyes==1) {
                vignette.saturation.Override(0f);
            }
            if (eyes==2) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}
