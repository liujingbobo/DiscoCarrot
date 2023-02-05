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
    private bool allPerfect;
    private bool block;
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
        block = false;
        phase = 0;
        clockwise = Random.Range(0, 2) == 1;
        MaxSampleTime = int.MaxValue;
    }

    public void UpdateState()
    {
        if (block) return;
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
                        var l = allPerfect ? ActionLevel.Perfect : ActionLevel.Good;
                        G.Indicator.Present(l);
                        block = true;
                        StartCoroutine(Success(l));
                    }
                    else
                    {
                        if (phase == 0)
                        {
                            MaxSampleTime = K.GetMaxSampleTime(kEvent, 1.5f);
                            allPerfect = true;
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

    IEnumerator Success(ActionLevel level)
    {
        yield return new WaitForSeconds(K.SampleTimeToTime((int) K.SamplePerBeat));
        G.StateMachine.Success(level);
        block = false;
    }

}
