using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ChangeSceneAfterCutscene : MonoBehaviour
{
    public float cutsceneLength;

    // Update is called once per frame
    private void Update()
    {
        cutsceneLength -= Time.deltaTime;
        if(cutsceneLength <= 0)
        {
            SceneManager.LoadScene(2);
        }
    }
}
