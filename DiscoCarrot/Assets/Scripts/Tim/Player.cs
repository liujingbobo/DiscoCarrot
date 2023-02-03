using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController cc;
    public float speed;

    [SpineAnimation] public string runAnimationName;
    [SpineAnimation] public string idleAnimationName;
    public SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState spineAnimationState;
    public Spine.Skeleton skeleton;

    void Start()
    {
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;
    }

    private void Update()
    {
        //Read Input and movement
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }

        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }

        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;
        }

        if (direction != Vector3.zero) cc.Move(direction * speed);
        else cc.Move(Vector3.zero);

        //animation
        if (spineAnimationState.GetCurrent(0).Animation.Name != runAnimationName && cc.velocity.magnitude > 0)
        {
            spineAnimationState.SetAnimation(0, runAnimationName, true);
        }
        
        if (spineAnimationState.GetCurrent(0).Animation.Name != idleAnimationName && cc.velocity.magnitude <= 0)
        {
            spineAnimationState.SetAnimation(0, idleAnimationName, true);
        }
        
        //face direction
        if(direction.x < 0) skeleton.ScaleX = -1;
        if(direction.x > 0) skeleton.ScaleX = 1;
        
    }
}