using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("HandCutscene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Level1()
    {
        SceneManager.LoadScene("HandCutscene");
    }

    public void Level2()
    {
        SceneManager.LoadScene("EyeCutscene");
    }

    public void Level3()
    {
        SceneManager.LoadScene("EarsCutscene");
    }

    public void Level4()
    {
        SceneManager.LoadScene("MouthCutscene");
    }

    public void Level5()
    {
        SceneManager.LoadScene("NoseLevel");
    }
}
