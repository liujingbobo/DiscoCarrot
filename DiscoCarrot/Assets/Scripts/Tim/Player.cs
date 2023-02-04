using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine;
using Event = Spine.Event;

public class Player : MonoBehaviour
{
    public CharacterController cc;
    public float speed;
    public Vector3 startPosition;
    private bool isPlayerMovable = false;
    
    public SkeletonAnimation skeletonAnimation;
    [SpineAnimation] public string idleAnimationName;
    [SpineAnimation] public string moveAnimationName;
    [SpineAnimation] public string sadAnimationName;
    [SpineAnimation] public string plow0AnimationName;
    [SpineAnimation] public string plow1AnimationName;
    [SpineAnimation] public string plantSeedAnimationName;
    [SpineAnimation] public string waterSeedAnimationName;
    [SpineAnimation] public string debugSeedAnimationName;
    [SpineAnimation] public string fertilize0SeedAnimationName;
    [SpineAnimation] public string fertilize1SeedAnimationName;
    [SpineAnimation] public string harvest0AnimationName;
    [SpineAnimation] public string harvest1AnimationName;
    
    public Spine.AnimationState spineAnimationState;
    public Spine.Skeleton skeleton;

    public PlayerAnimName currentAnimState;
    public int failSaveBeatCount = -1;

    void Start()
    {
        startPosition = transform.position;
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;
        skeletonAnimation.AnimationState.Event += HandleAnimationStateEvent;
        SwitchToAnimState(PlayerAnimName.Idle);
        GameEvents.OnOneBeatPassed += OnOneBeatPassed;
    }

    private void HandleAnimationStateEvent(TrackEntry trackentry, Event e)
    {
        //Trigger SFX
        Debug.Log($"animation event fired: {e}");
        var eventName = e.Data.Name;
        switch (eventName)
        {
            case "feed": //SoundEffectManager.singleton.PlaySFX();
                break;
            case "harvest": SoundEffectManager.singleton.PlaySFX(SoundEffectManager.SoundEffectName.harvest);
                break;
            case "hoeing": 
                SoundEffectManager.singleton.PlaySFX(SoundEffectManager.SoundEffectName.playerHoar);
                SoundEffectManager.singleton.PlaySFX(SoundEffectManager.SoundEffectName.hoeing);
                break;
            case "insecticide": 
                SoundEffectManager.singleton.PlaySFX(SoundEffectManager.SoundEffectName.playerHoar);
                SoundEffectManager.singleton.PlaySFX(SoundEffectManager.SoundEffectName.insecticide);
                break;
            case "miss": SoundEffectManager.singleton.PlaySFX(SoundEffectManager.SoundEffectName.miss);
                break;
            case "move": SoundEffectManager.singleton.PlaySFX(SoundEffectManager.SoundEffectName.move);
                break;
            case "sowing": 
                SoundEffectManager.singleton.PlaySFX(SoundEffectManager.SoundEffectName.playerHoar);
                SoundEffectManager.singleton.PlaySFX(SoundEffectManager.SoundEffectName.plantSeed);
                break;
            case "watering": 
                SoundEffectManager.singleton.PlaySFX(SoundEffectManager.SoundEffectName.playerHoar);
                SoundEffectManager.singleton.PlaySFX(SoundEffectManager.SoundEffectName.water);
                break;
        }
    }

    private void OnOneBeatPassed()
    {
        if (currentAnimState != PlayerAnimName.Idle && currentAnimState != PlayerAnimName.Move)
        {
            if(failSaveBeatCount == 0) SwitchToAnimState(PlayerAnimName.Idle);
            if (failSaveBeatCount > 0) failSaveBeatCount -= 1;
        }
    }

    public void ResetPlayer()
    {
        cc.Move(startPosition - transform.position);
        SwitchToAnimState(PlayerAnimName.Idle);
        SetPlayerMovable(false);
    }

    public void SetPlayerMovable(bool movable)
    {
        SwitchToAnimState(PlayerAnimName.Idle);
        isPlayerMovable = movable;
    }
    
    private void Update()
    {
        MovableStateUpdate();
    }

    private void MovableStateUpdate()
    {
        if (!isPlayerMovable) return;
        if (currentAnimState == PlayerAnimName.Idle || currentAnimState == PlayerAnimName.Move)
        {
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
            if (currentAnimState == PlayerAnimName.Idle && cc.velocity.magnitude > 0)
            {
                SwitchToAnimState(PlayerAnimName.Move);
            }

            if (currentAnimState == PlayerAnimName.Move && cc.velocity.magnitude <= 0)
            {
                SwitchToAnimState(PlayerAnimName.Idle);
            }

            //face direction
            if (direction.x < 0) skeleton.ScaleX = -1;
            if (direction.x > 0) skeleton.ScaleX = 1;
        }
    }

    public void SwitchToAnimState(PlayerAnimName targetAnimName, Transform teleportPoint = null)
    {
        currentAnimState = targetAnimName;
        if (teleportPoint != null) cc.Move(teleportPoint.position - transform.position);
        PlayAnim(targetAnimName);
    }

    private void PlayAnim(PlayerAnimName animName)
    {
        failSaveBeatCount = 1;
        skeleton.ScaleX = 1;
        switch (animName)
        {
            case PlayerAnimName.Idle:
                spineAnimationState.SetAnimation(0, idleAnimationName, true);
                break;
            case PlayerAnimName.Move:
                spineAnimationState.SetAnimation(0, moveAnimationName, true);
                break;
            case PlayerAnimName.Sad:
                spineAnimationState.SetAnimation(0, sadAnimationName, false);
                break;
            case PlayerAnimName.PlowUp:
                spineAnimationState.SetAnimation(0, plow0AnimationName, false);
                break;
            case PlayerAnimName.PlowDown:
                spineAnimationState.SetAnimation(0, plow1AnimationName, false);
                break;
            case PlayerAnimName.PlantSeed:
                spineAnimationState.SetAnimation(0, plantSeedAnimationName, false);
                break;
            case PlayerAnimName.Watering:
                spineAnimationState.SetAnimation(0, waterSeedAnimationName, false);
                break;
            case PlayerAnimName.Debugging:
                spineAnimationState.SetAnimation(0, debugSeedAnimationName, false);
                break;
            case PlayerAnimName.ReadyFertilize:
                spineAnimationState.SetAnimation(0, fertilize0SeedAnimationName, false);
                break;
            case PlayerAnimName.Fertilize:
                spineAnimationState.SetAnimation(0, fertilize1SeedAnimationName, false);
                break;
            case PlayerAnimName.Harvest0:
                spineAnimationState.SetAnimation(0, harvest0AnimationName, false);
                break;
            case PlayerAnimName.Harvest1:
                spineAnimationState.SetAnimation(0, harvest1AnimationName, false);
                break;
        }
    }
}