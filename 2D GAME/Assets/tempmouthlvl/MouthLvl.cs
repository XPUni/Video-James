using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MouthLvl : MonoBehaviour
{
    public GameObject egg0;
    public GameObject egg1;
    public GameObject flour0;
    public GameObject flour1;
    public GameObject sugar0;
    public GameObject sugar1;
    public GameObject milk0;
    public GameObject milk1;

    public bool gotEgg = false;
    public bool gotMilk = false; //haha like the ad
    public bool gotSugar = false;
    public bool gotFlour = false;
    public GameObject ingredients;
    public GameObject returnText;

    public bool complete = false;

    public float lvlTimer = 60f;
    public float lvlTime = 0f;
    public TextMeshProUGUI timeText;
    public GameObject clock;

    // Update is called once per frame
    void Update()
    {
        if (!complete && clock == true)
        {
            Timer();
        }

        if (gotEgg && gotFlour && gotMilk && gotSugar)
        {
            ingredients.SetActive(false);
            returnText.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.gameObject.name)
        {
            case "Egg":
                collision.gameObject.SetActive(false);
                egg0.SetActive(false);
                egg1.SetActive(true);
                gotEgg = true;
                break;
            case "Flour":
                collision.gameObject.SetActive(false);
                flour0.SetActive(false);
                flour1.SetActive(true);
                gotFlour = true;
                break;
            case "Sugar":
                collision.gameObject.SetActive(false);
                sugar0.SetActive(false);
                sugar1.SetActive(true);
                gotSugar = true; 
                break;
            case "Milk":
                collision.gameObject.SetActive(false);
                milk0.SetActive(false);
                milk1.SetActive(true);
                gotMilk = true; 
                break;
            case "Bowl":
                if (gotEgg && gotFlour && gotMilk && gotSugar)
                {
                    complete = true;
                    collision.gameObject.SetActive(false);
                    clock.SetActive(false);
                }
                break;
            case "Mouth":
                if (complete)
                {
                    collision.gameObject.SetActive(false);
                }
                break;
        }
    }

    void Timer()
    {
        if (clock.activeSelf)
        {
            lvlTimer -= Time.deltaTime;
            timeText.text = (Mathf.Floor(lvlTimer/60).ToString()+':'+Mathf.Floor(lvlTimer%60).ToString().PadLeft(2,'0'));
            //timeText.text = (Mathf.RoundToInt(lvlTimer.ToString()));

            if (lvlTimer <= 0.0f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        


        /*lvlTime += Time.deltaTime;
        timeText.text = (Mathf.RoundToInt(lvlTime.ToString());

        if (lvlTime > lvlTimer)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            lvlTime = 0f;
        }*/
    }
}
