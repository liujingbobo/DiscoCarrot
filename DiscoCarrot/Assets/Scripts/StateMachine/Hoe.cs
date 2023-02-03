using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Hoe : MonoBehaviour, IState
{
    private bool downPressed = false;
    private int MaxSampleTime;
    
    public void Enter()
    {
        
    }

    public void Exit()
    {
        
    }

    public void Reset()
    {
        downPressed = false;
        MaxSampleTime = 0;
    }

    public void Update()
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
                
            }
        }

        if (downPressed)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (allValidKeyDown.Count > 1)
                {
                    // Failed
                }
            }
        }

        if (K.CurrentSampleTime > MaxSampleTime)
        {
            // Failed
        }
    }
}
