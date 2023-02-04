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
    }

    public void UpdateState()
    {
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
                        G.Indicator.UpdateState(ArrowState.Perfect);
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


        if (downPressed && K.CurrentSampleTime > MaxSampleTime)
        {
            // Failed
            G.StateMachine.Fail();
        }
    }
}
