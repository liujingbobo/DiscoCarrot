using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class FollowCameraRotationX : MonoBehaviour
{
    public static Camera targetCamera = null;
    void Update()
    {
        if (targetCamera == null) targetCamera = Camera.main;
        transform.rotation = Quaternion.AngleAxis(targetCamera.transform.eulerAngles.x, Vector3.right);
    }
}
