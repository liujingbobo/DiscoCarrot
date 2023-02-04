using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SimpleFollowTargetCamera : MonoBehaviour
{
    public Camera camera; 
    public Transform target;
    public bool setDistance;
    public Vector3 distance;
    public Vector3 aimOffset;

    private void Update()
    {
        if(setDistance) camera.transform.position = target.position + distance;
        var aimDirection = (target.position  + aimOffset) - camera.transform.position;
        camera.transform.localRotation = Quaternion.LookRotation(aimDirection);
    }
}
