using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class S_Hoe : MonoBehaviour, IState
{
    private bool downPressed = false;
    private int MaxSampleTime;
    private int ExpectSample;
    private PressLevel firstLevel;
    private bool block = false;
    
    public void Enter()
    {
        Reset();
    }

    public void Exit()
    {
    }

    public void Reset()
    {
        downPressed = false;
        MaxSampleTime = 0;
        ExpectSample = 0;
        block = false;
    }

    public void UpdateState()
    {
        if (block) return;
        
        var allValidKeyDown = K.GetAllValidKeyDown();

        if (allValidKeyDown.Count > 0)
        {
            if (!downPressed)
            {
                var level = K.GetCurrentArrowLevel();
            
                if (!Input.GetKeyDown(KeyCode.UpArrow) || allValidKeyDown.Count > 1 || level == PressLevel.Miss)
                {
                    // Failed
                    G.StateMachine.Fail();
                }
                else
                {
                    // UpdateState
                    // GameEvents.OnFarmActionDone.Invoke();
                    print("1 state success");
                    downPressed = true;
                    G.Indicator.UpdateState(ArrowState.Perfect);
                    MaxSampleTime = K.GetMaxSampleTime(K.GetClosestDownBeatEvent(), 1);
                    ExpectSample =(int) (K.GetClosestDownBeatEvent().EndSample + K.SamplePerBeat * 1);
                    firstLevel = level;
                    GameManager.singleton.sharedContext.player.SwitchToAnimState(PlayerAnimName.PlowUp);
                }
            }else if (downPressed)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    var level = K.GetArrowLevel(ExpectSample);

                    if (allValidKeyDown.Count > 1 || level == PressLevel.Miss)
                    {
                        // Failed
                        G.StateMachine.Fail();
                    }
                    else
                    {
                        // Success
                        GameManager.singleton.sharedContext.player.SwitchToAnimState(PlayerAnimName.PlowDown);
                        G.Indicator.UpdateState(level.ToArrowState());
                        var actionLevel = (firstLevel, level) switch
                        {
                            (PressLevel.Perfect, PressLevel.Perfect) => ActionLevel.Perfect,
                            _ => ActionLevel.Good
                        };
                        G.Indicator.Present(actionLevel);
                        block = true;
                        StartCoroutine(Success(actionLevel));
                    }
                }
                else
                {
                    // Failed
                    G.StateMachine.Fail();
                }
            }
        }


        if (downPressed && K.CurrentSampleTime > MaxSampleTime)
        {
            // Failed
            G.StateMachine.Fail();
        }
    }

    IEnumerator Success(ActionLevel level)
    {
        yield return new WaitForSeconds(K.SampleTimeToTime((int) K.SamplePerBeat));
        G.StateMachine.Success(level);
        block = false;
    }
}
