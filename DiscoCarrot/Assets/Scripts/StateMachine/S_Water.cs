using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Water : MonoBehaviour, IState
{
    private bool downPressed = false;
    private int ExpectFinalSampleTime;
    public void Enter()
    {
        G.Indicator.SwitchTo(PlayerFarmAction.WaterPlant);
        Reset();
    }

    public void Exit()
    {
    }

    public void Reset()
    {
        downPressed = false;
        ExpectFinalSampleTime = 0;
    }

    public void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            var allValidKeyDown = K.GetAllValidKeyDown();

            var level = K.GetCurrentArrowLevel();

            var kEvent = K.GetClosestDownBeatEvent();
            
            if (allValidKeyDown.Count > 1 || level == PressLevel.Miss)
            {
                // Failed
            }
            else
            {
                downPressed = true;
                ExpectFinalSampleTime = kEvent.EndSample + ( 2 * (int)K.SamplePerBeat);
                // GameEvents.OnFarmActionDone.Invoke();
                return;
            }
        }

        if (Input.GetKeyUp(KeyCode.DownArrow) && downPressed)
        {
            var level = K.GetArrowLevel(ExpectFinalSampleTime);

            if (level == PressLevel.Miss)
            {
                // Failed
            }
            else
            {
                // Success
            }
        }
    }
}
