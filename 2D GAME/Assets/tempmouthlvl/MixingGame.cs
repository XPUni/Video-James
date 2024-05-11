using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MixingGame : MonoBehaviour
{
    public void Clicked()
    {
        Debug.Log("Clicked");
        Destroy(gameObject);
    }
}
