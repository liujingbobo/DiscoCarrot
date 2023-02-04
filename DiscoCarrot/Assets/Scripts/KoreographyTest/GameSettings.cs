using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using SonicBloom.Koreo;
using UnityEngine;

public class GameSettings : SerializedMonoBehaviour
{
    public Dictionary<PressLevel, int> ArrowScore;
    public Dictionary<CarrotLevel, int> CarrotScore;
    public Dictionary<ActionLevel, int> ActionLevel;
    
    public Dictionary<PressLevel, float> ArrowLevelThreshold;

    [EventID]public string DownBeatEvent;
    [EventID]public string UpBeatEvent;

    public Dictionary<string, float> SamplesPerBeatForSong;
}