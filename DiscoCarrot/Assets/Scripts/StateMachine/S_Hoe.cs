using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class S_Hoe : MonoBehaviour, IState
{
    private bool downPressed = false;
    private int MaxSampleTime;
    
    public void Enter()
    {
        G.Indicator.SwitchTo(PlayerFarmAction.PlowLand);
    }

    public void Exit()
    {
        
    }

    public void Reset()
    {
        downPressed = false;
        MaxSampleTime = 0;
    }

    public void UpdateState()
    {
        var allValidKeyDown = K.GetAllValidKeyDown();

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            var level = K.GetCurrentArrowLevel();
            
            if (allValidKeyDown.Count > 1 || level == ArrowLevel.Miss)
            {
                // Failed
            }
            else
            {
                // GameEvents.OnFarmActionDone.Invoke();
            }
        }

        if (downPressed)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                var level = K.GetCurrentArrowLevel();

                if (allValidKeyDown.Count > 1 || level == ArrowLevel.Miss)
                {
                    // Failed
                }
                else
                {
                    
                }
            }
        }

        if (K.CurrentSampleTime > MaxSampleTime)
        {
            // Failed
        }
    }
}
