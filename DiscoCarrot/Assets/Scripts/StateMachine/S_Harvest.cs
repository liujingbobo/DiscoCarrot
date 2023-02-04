using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Harvest : MonoBehaviour, IState
{
    private int phase;
    private int MaxSampleTime;
    private int expectNext;
    public void Enter()
    {
        Reset();
    }

    public void Exit()
    {
    }

    public void Reset()
    {
        phase = 0;
        expectNext = 0;
    }

    public void UpdateState()
    {
        var isEven = phase % 2 == 0;

        if (isEven)
        {
            var allKeys = K.GetAllValidKeyDown();

            if (allKeys.Count > 0)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) && allKeys.Count == 1)
                {
                    var level = K.GetCurrentArrowLevel(true);

                    if (level == PressLevel.Miss)
                    {
                        // failed
                        G.StateMachine.Fail();
                    }
                    else
                    {
                        expectNext = K.GetClosestDownBeatEvent().EndSample + (int) (K.SamplePerBeat * 0.5f);
                        phase++;
                        // Update UI
                        G.Indicator.UpdateState(ArrowState.Perfect);
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
            var allKeys = K.GetAllValidKeyDown();

            if (allKeys.Count > 0)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) && allKeys.Count == 1)
                {
                    var level = K.GetCurrentArrowLevel(false);

                    if (level == PressLevel.Miss)
                    {
                        // failed
                        G.StateMachine.Fail();
                    }
                    else
                    {
                        if (phase == 7)
                        {
                            // success
                            G.Indicator.UpdateState(level.ToArrowState());
                            G.StateMachine.Success(ActionLevel.Perfect);
                        }
                        else
                        {
                            expectNext = K.GetClosestUpBeatEvent().EndSample + (int) (K.SamplePerBeat * 0.5f);
                            phase++;
                            // Update UI
                            G.Indicator.UpdateState(ArrowState.Perfect);
                        }
                    }
                }
                else
                {
                    // Failed
                    G.StateMachine.Fail();
                }
            }
        }
        
        if (phase > 0 && K.CurrentSampleTime > expectNext)
        {
            // Failed
            G.StateMachine.Fail();
        }
    }
}
