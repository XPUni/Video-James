using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class EyeTracker : MonoBehaviour
{
    PostProcessVolume m_Volume;
    ColorGrading m_Grayscale;
    private int eyes;
    private bool triggerThisFrame = false;
    void Start()
        {
            eyes = 0;
            // Create an instance of a vignette
            m_Grayscale = ScriptableObject.CreateInstance<ColorGrading>();
            m_Grayscale.enabled.Override(true);
            m_Grayscale.saturation.Override(-100f);
            // Use the QuickVolume method to create a volume with a priority of 100, and assign the vignette to this volume
            m_Volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, m_Grayscale);
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
                m_Grayscale.enabled.Override(false);
            }
            if (eyes==2) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}
