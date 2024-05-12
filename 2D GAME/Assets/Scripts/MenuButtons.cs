using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Level1()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Level2()
    {
        SceneManager.LoadScene("EyeLevel");
    }

    public void Level3()
    {
        SceneManager.LoadScene("Ears");
    }

    public void Level4()
    {
        SceneManager.LoadScene("mouth");
    }

    public void Level5()
    {
        SceneManager.LoadScene("NoseLevel");
    }
}
