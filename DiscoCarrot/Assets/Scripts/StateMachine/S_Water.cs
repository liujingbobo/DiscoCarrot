using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Water : MonoBehaviour, IState
{
    private bool downPressed = false;
    private int ExpectFinalSampleTime;
    private int MaxSampleTime;
    private bool block;
    private PressLevel firstLevel;
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
        ExpectFinalSampleTime = 0;
        MaxSampleTime = int.MaxValue;
        block = false;
    }

    public void UpdateState()
    {
        if (block) return;
        if (!downPressed)
        {
            var allValidKeyDown = K.GetAllValidKeyDown();

            if (allValidKeyDown.Count > 0)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    var level = K.GetCurrentArrowLevel();

                    var kEvent = K.GetClosestDownBeatEvent();
            
                    if (allValidKeyDown.Count > 1 || level == PressLevel.Miss)
                    {
                        // Failed
                        G.StateMachine.Fail();
                    }
                    else
                    {
                        GameManager.singleton.sharedContext.player.SwitchToAnimState(PlayerAnimName.Watering);
                        downPressed = true;
                        ExpectFinalSampleTime = kEvent.EndSample + (int)( 1.5 * K.SamplePerBeat);
                        MaxSampleTime = K.GetMaxSampleTime(kEvent, 1.5f);
                        firstLevel = level;
                        // UpdateResult
                        G.Indicator.UpdateState(level.ToArrowState());
                    }
                }
                else
                {
                    // Failed
                    G.StateMachine.Fail();
                }
            }

        }
        else
        {
            var allValidKeyUp = K.GetAllValidKeyUp();

            if (allValidKeyUp.Count > 0)
            {
                if (Input.GetKeyUp(KeyCode.DownArrow))
                {
                    var level = K.GetArrowLevel(ExpectFinalSampleTime);

                    if (level == PressLevel.Miss)
                    {
                        // Failed
                        G.StateMachine.Fail();
                    }
                    else
                    {
                        // Success
                        G.Indicator.UpdateState(level.ToArrowState());
                        var l = (firstLevel, level) switch
                        {
                            (PressLevel.Perfect, PressLevel.Perfect) => ActionLevel.Perfect,
                            _ => ActionLevel.Good
                        };
                        block = true;
                        G.Indicator.Present(l);
                        StartCoroutine(Success(l));
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
        yield return new WaitForSeconds(0.5f * (float) (60 / K.BeatsPerMinute));
        G.StateMachine.Success(level);
        block = false;
    }
}
