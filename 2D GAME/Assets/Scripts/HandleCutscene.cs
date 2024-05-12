using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandleCutscene : MonoBehaviour
{
    public string sceneName;
    public float duration;

    private void Update()
    {
        duration -= Time.deltaTime;
        if(duration <= 0)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
