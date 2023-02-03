using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class S_Debug : MonoBehaviour, IState
{
    private int progress;
    private int expectedSampleTime;
    private bool clockwise;
    
    public void Enter()
    {
        progress = 0;
        expectedSampleTime = 0;
        clockwise = Random.Range(0, 2) == 1;
    }

    public void Exit()
    {
    }

    public void Reset()
    {
    }

    public void Update()
    {
        if (clockwise)
        {
            var targetKey = progress switch
            {
                0 => KeyCode.UpArrow,
                1 => KeyCode.RightArrow,
                2 => KeyCode.DownArrow,
                3 => KeyCode.LeftArrow
            };
        }
        else
        {
            
        }
    }
}
