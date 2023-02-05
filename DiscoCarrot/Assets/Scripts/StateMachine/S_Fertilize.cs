using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Fertilize : MonoBehaviour, IState
{
    private bool downPressed;
    private int expectedSampleTime;
    private int maxSampleTime;
    private PressLevel firstLevel;
    private bool block;
    public void Enter()
    {
        G.Indicator.SwitchTo(PlayerFarmAction.FertilizePlant);
        Reset();
    }
    public void Exit()
    {
    }
    public void Reset()
    {
        downPressed = false;
        expectedSampleTime = 0;
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
                        downPressed = true;
                        expectedSampleTime = kEvent.EndSample + (int)(0.5 * K.SamplePerBeat);
                        maxSampleTime = K.GetMaxSampleTime(kEvent, 0.5f);
                        firstLevel = level;
                        // UpdateState
                        G.Indicator.UpdateState(level.ToArrowState());
                        return;
                    }
                }
                else
                {
                    // Failed
                    G.StateMachine.Fail();
                }

            }else if (downPressed)
            {

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    var level = K.GetArrowLevel(expectedSampleTime);
            
                    if (allValidKeyDown.Count > 1 || level == PressLevel.Miss)
                    {
                        // Failed
                        G.StateMachine.Fail();
                    }
                    else
                    {
                        // Success
                        // GameEvents.OnFarmActionDone.Invoke();
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

        
        if (downPressed && K.CurrentSampleTime > maxSampleTime)
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
