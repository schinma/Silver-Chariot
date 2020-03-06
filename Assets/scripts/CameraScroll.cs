using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    public float cameraSpeed;

    private bool stop = false;

    // Update is called once per frame
    void Update()
    {
        if (!stop)
        {
            var cameraPosition = gameObject.transform.position;
            cameraPosition.x += Time.deltaTime * cameraSpeed;
            gameObject.transform.position = cameraPosition;
        }
    }

    public void StopCameraScrolling()
    {
        stop = true;
    }

    public void StartCameraScrolling()
    {
        stop = false;
    }
}
