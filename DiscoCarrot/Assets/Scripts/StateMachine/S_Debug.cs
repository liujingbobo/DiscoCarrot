using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class S_Debug : MonoBehaviour, IState
{
    private int phase;
    private int expectedSampleTime;
    private bool clockwise;
    
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
        expectedSampleTime = 0;
        clockwise = Random.Range(0, 2) == 1;
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
        
        if (Input.GetKeyDown(targetKey))
        {
            var isDownBeat = phase == 0 || phase == 2;
                
            var allValidKeyDown = K.GetAllValidKeyDown();

            var level = K.GetCurrentArrowLevel(isDownBeat);

            var kEvent = isDownBeat ? K.GetClosestDownBeatEvent() : K.GetClosestUpBeatEvent();
            
            if (allValidKeyDown.Count > 1 || level == ArrowLevel.Miss)
            {
                // Failed
            }
            else
            {
                if (phase == 3)
                {
                    // success
                }
                else
                {
                    phase++;
                    expectedSampleTime = kEvent.EndSample + ((int)K.SamplePerBeat / 2);
                }
            }
        }
    }

}
