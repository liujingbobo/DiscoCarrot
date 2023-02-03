using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;
using UnityEngine;

public static class K
{
    public static Koreographer koreographer;
    public static SimpleMusicPlayer musicPlayer;
    public static string currentClip => musicPlayer.GetCurrentClipName();
    public static int CurrentSampleTime => koreographer.GetMusicSampleTime();
    public static int TotalSampleTime => musicPlayer.GetTotalSampleTimeForClip(currentClip);
    public static int SampleRate => koreographer.GetMusicSampleRate();
    public static float SampleTimeToTime(int sampleTime)
    {
        var temp = (float) sampleTime / SampleRate;
        return temp;
    }

    public static KoreographyEvent GetClosestDownBeatEvent()
    {
        int min = CurrentSampleTime - SampleRate;
        int max = CurrentSampleTime + SampleRate;

        var validEvents = koreographer.GetAllEventsInRange(currentClip, G.Settings.DownBeatEvent, min, max);

        int dis = int.MaxValue;
        int minDisIndex = 0;
        for(int i = 0; i < validEvents.Count; i++)
        {
            var eEvent = validEvents[i];
            var d = Math.Abs(eEvent.EndSample - CurrentSampleTime);
            if ( d <= dis)
            {
                dis = d;
                minDisIndex = i;
            }
        }

        return validEvents[minDisIndex];
    }   
    
    public static KoreographyEvent GetClosestUpBeatEvent()
    {
        int min = CurrentSampleTime - SampleRate;
        int max = CurrentSampleTime + SampleRate;

        var validEvents = koreographer.GetAllEventsInRange(currentClip, G.Settings.UpBeatEvent, min, max);

        int dis = int.MaxValue;
        int minDisIndex = 0;
        for(int i = 0; i < validEvents.Count; i++)
        {
            var eEvent = validEvents[i];
            var d = Math.Abs(eEvent.EndSample - CurrentSampleTime);
            if ( d <= dis)
            {
                dis = d;
                minDisIndex = i;
            }
        }

        return validEvents[minDisIndex];
    }
    
    
}
