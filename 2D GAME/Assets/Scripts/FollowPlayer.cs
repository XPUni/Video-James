using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] public GameObject player;
    [SerializeField] public Camera camera;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = camera.WorldToViewportPoint(player.transform.position);
        if (screenPos.x < 0) {
            camera.transform.position = new Vector3(camera.transform.position.x - 30, camera.transform.position.y, camera.transform.position.z);
        } else if (screenPos.x > 1) {
            camera.transform.position = new Vector3(camera.transform.position.x + 30, camera.transform.position.y, camera.transform.position.z);
        }
    }
}
