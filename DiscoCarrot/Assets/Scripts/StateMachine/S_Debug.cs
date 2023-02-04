using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class S_Debug : MonoBehaviour, IState
{
    private int phase;
    private bool clockwise;
    private int MaxSampleTime;

    public void Enter()
    {
        G.Indicator.SwitchTo(PlayerFarmAction.DebugPlant);
        Reset();
    }

    public void Exit()
    {
        Reset();
    }

    public void Reset()
    {
        phase = 0;
        clockwise = Random.Range(0, 2) == 1;
        MaxSampleTime = int.MaxValue;
    }

    public void UpdateState()
    {
        var targetKey = (phase, clockwise) switch
        {
            (0, _) => KeyCode.UpArrow,
            (1, true) => KeyCode.RightArrow,
            (1, false) => KeyCode.LeftArrow,
            (2, _) => KeyCode.DownArrow,
            (3, true) => KeyCode.LeftArrow,
            (3, false) => KeyCode.RightArrow,
        };
        
        var allValidKeyDown = K.GetAllValidKeyDown();

        if (allValidKeyDown.Count > 0)
        {
            if (Input.GetKeyDown(targetKey))
            {
                var isDownBeat = phase == 0 || phase == 2;

                var level = K.GetCurrentArrowLevel(isDownBeat);

                var kEvent = isDownBeat ? K.GetClosestDownBeatEvent() : K.GetClosestUpBeatEvent();
            
                if (allValidKeyDown.Count > 1 || level == PressLevel.Miss)
                {
                    // Failed
                    G.StateMachine.Fail();
                }
                else
                {
                    if (phase == 3)
                    {
                        // success
                        G.Indicator.UpdateState(level.ToArrowState());
                        G.StateMachine.Success(ActionLevel.Perfect);
                    }
                    else
                    {
                        if (phase == 0)
                        {
                            MaxSampleTime = K.GetMaxSampleTime(kEvent, 1.5f);
                        }
                    
                        phase++;
                        // UpdateState
                        G.Indicator.UpdateState(level.ToArrowState());
                    }
                }
            }
            else
            {
                // Failed
                G.StateMachine.Fail();
            }
        }

        if (phase > 0 && K.CurrentSampleTime > MaxSampleTime)
        {
            // Failed
            G.StateMachine.Fail();
        }
    }

}
