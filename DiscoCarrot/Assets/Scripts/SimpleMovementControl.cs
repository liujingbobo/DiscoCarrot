using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovementControl : MonoBehaviour
{
    public CharacterController cc;
    public float speed;
    
    private void Update()
    {
        //Read Input
        if (Input.GetKey(KeyCode.W))
        {
            cc.Move(Vector3.forward * speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            cc.Move(Vector3.left * speed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            cc.Move(Vector3.right * speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            cc.Move(Vector3.back * speed);
        }
        
    }
}
