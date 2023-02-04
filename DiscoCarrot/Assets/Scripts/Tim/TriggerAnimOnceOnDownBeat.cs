using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class TriggerAnimOnceOnDownBeat : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState spineAnimationState;
    [SpineAnimation] public string animName;
    void Start()
    {
        spineAnimationState = skeletonAnimation.AnimationState;
        GameEvents.OnDownBeat += OnDownBeat;
    }

    private void OnDownBeat()
    {
        spineAnimationState.SetAnimation(0, animName, false);
    }
}
