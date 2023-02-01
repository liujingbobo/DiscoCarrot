using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollowTargetCamera : MonoBehaviour
{
    public Camera camera; 
    public Transform target;
    public Vector3 distance;
    public Vector3 aimOffset;

    private void Update()
    {
        camera.transform.position = target.position + distance;
        var aimDirection = (target.position  + aimOffset) - camera.transform.position;
        camera.transform.localRotation = Quaternion.LookRotation(aimDirection);
    }
}
