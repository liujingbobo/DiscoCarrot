using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class G
{
    public static GameSettings Settings;
    public static OperationIndicator Indicator;
    public static FarmingStateMachine StateMachine;
    public static float GetThreshold(PressLevel level)
    {
        return Settings.ArrowLevelThreshold[level];
    }
}