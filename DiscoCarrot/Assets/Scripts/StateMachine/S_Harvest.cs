using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Harvest : MonoBehaviour, IState
{
    private int phase;
    private int MaxSampleTime;
    private int expectNext;
    private bool allPerfect;
    private bool block;
    private int maxNext;
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
        if (block) return;
        
        var isEven = phase % 2 == 0;

        if (isEven)
        {
            var allKeys = K.GetAllValidKeyDown();

            if (allKeys.Count > 0)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) && allKeys.Count == 1)
                {
                    var level = K.GetCurrentArrowLevel(true);

                    var kEvent = K.GetClosestDownBeatEvent();
                    if (level == PressLevel.Miss)
                    {
                        // failed
                        G.StateMachine.Fail();
                    }
                    else
                    {
                        GameManager.singleton.sharedContext.player.SwitchToAnimState(PlayerAnimName.Harvest0);
                        if (phase == 0) allPerfect = true;
                        expectNext = kEvent.EndSample + (int) (K.SamplePerBeat * 0.5f);
                        maxNext = K.GetMaxSampleTime(kEvent, 0.5f);
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

                    var kEvent = K.GetClosestUpBeatEvent();
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
                            GameManager.singleton.sharedContext.player.SwitchToAnimState(PlayerAnimName.Harvest1);
                            G.Indicator.UpdateState(level.ToArrowState());
                            StartCoroutine(Success(allPerfect ? ActionLevel.Perfect : ActionLevel.Good));
                            return;
                        }
                        else
                        {
                            GameManager.singleton.sharedContext.player.SwitchToAnimState(PlayerAnimName.Harvest0);
                            expectNext = kEvent.EndSample + (int) (K.SamplePerBeat * 0.5f);
                            maxNext = K.GetMaxSampleTime(kEvent, 0.5f);
                            phase++;
                            // Update UI
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
        }
        
        if (phase > 0 && K.CurrentSampleTime > maxNext)
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
