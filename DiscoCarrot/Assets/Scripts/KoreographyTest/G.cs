using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class G
{
    public static GameSettings Settings;
    public static OperationIndicator Indicator;
    public static float GetThreshold(ArrowLevel level)
    {
        return Settings.ArrowLevelThreshold[level];
    }
    
}