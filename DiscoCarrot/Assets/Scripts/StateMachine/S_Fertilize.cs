using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Fertilize : MonoBehaviour, IState
{
    private bool downPressed;
    private int expectedSampleTime;
    private int maxSampleTime;
    
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
    }
    public void UpdateState()
    {
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
                        G.StateMachine.Success(ActionLevel.Perfect);
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
}