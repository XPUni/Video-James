using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] public GameObject player;
    [SerializeField] public Camera camera;
    private Vector3 start;
    private Vector3 target;
    private float starttime;
    private float targettime;
    void Start()
    {
        start = camera.transform.position;
        target = camera.transform.position;
        starttime = Time.time;
        targettime = Time.time;
    }

    float Easing(float begin, float end, float current) {
        float t = (current-begin)/(end-begin);
        return 3*t*t-2*Mathf.Pow(t,3);
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = camera.WorldToViewportPoint(player.transform.position);
        if (starttime == targettime) {
            if (screenPos.x < 0) {
                target = new Vector3(camera.transform.position.x - 32, camera.transform.position.y, camera.transform.position.z);
                start = camera.transform.position;
                starttime = Time.time;
                targettime = Time.time + 0.45f;
            } else if (screenPos.x > 1) {
                target = new Vector3(camera.transform.position.x + 32, camera.transform.position.y, camera.transform.position.z);
                start = camera.transform.position;
                starttime = Time.time;
                targettime = Time.time + 0.45f;
            }
            if (screenPos.y < 0.2) {
                target = new Vector3(camera.transform.position.x, camera.transform.position.y - 15, camera.transform.position.z);
                start = camera.transform.position;
                starttime = Time.time;
                targettime = Time.time + 0.2f;
            } else if (screenPos.y > 1) {
                target = new Vector3(camera.transform.position.x, camera.transform.position.y + 15, camera.transform.position.z);
                start = camera.transform.position;
                starttime = Time.time;
                targettime = Time.time + 0.2f;
            }
        }
        if (targettime > Time.time) {
            camera.transform.position = Vector3.Lerp(start,target,Easing(starttime,targettime,Time.time));
        } else {
            camera.transform.position = target;
            starttime = targettime;
        }
    }
}
