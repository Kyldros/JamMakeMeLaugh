using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotation : MonoBehaviour
{
    public void RotateToCamera()
    {
        Camera cam = Camera.main;
        if (cam != null)
        {
            Vector3 cameraRotation = cam.transform.eulerAngles;
            transform.eulerAngles = cameraRotation;
        }
    }

    private void Update()
    {
        RotateToCamera();
    }
}
