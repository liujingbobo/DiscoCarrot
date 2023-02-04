using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Fertilize : MonoBehaviour, IState
{
    private bool downPressed;
    private int expectedSampleTime;
    
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
        if (!downPressed && Input.GetKeyDown(KeyCode.DownArrow))
        {
            var allValidKeyDown = K.GetAllValidKeyDown();
            
            var level = K.GetCurrentArrowLevel();
            
            var kEvent = K.GetClosestDownBeatEvent();
            
            if (allValidKeyDown.Count > 1 || level == ArrowLevel.Miss)
            {
                // Failed
            }
            else
            {
                downPressed = true;
                expectedSampleTime = kEvent.EndSample + (int)(0.5 * K.SamplePerBeat);
                return;
            }
        }
        
        if (downPressed && Input.GetKeyDown(KeyCode.DownArrow))
        {
            var allValidKeyDown = K.GetAllValidKeyDown();
            
            var level = K.GetArrowLevel(expectedSampleTime);
            
            if (allValidKeyDown.Count > 1 || level == ArrowLevel.Miss)
            {
                // Failed
            }
            else
            {
                // GameEvents.OnFarmActionDone.Invoke();
            }
        }
    }
}
