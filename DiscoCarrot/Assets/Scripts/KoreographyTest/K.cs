using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;
using UnityEngine;

// 
public static class K
{
    public static Koreographer koreographer;
    public static SimpleMusicPlayer musicPlayer;
    public static string currentClip => musicPlayer.GetCurrentClipName();
    public static int CurrentSampleTime => koreographer.GetMusicSampleTime();
    public static int TotalSampleTime => musicPlayer.GetTotalSampleTimeForClip(currentClip);
    public static int SampleRate => koreographer.GetMusicSampleRate();
    public static float SamplePerBeat => G.Settings.SamplesPerBeatForSong[currentClip];
    public static double BeatsPerMinute => koreographer.GetMusicBPM();
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
            var d = Math.Abs(eEvent.StartSample - CurrentSampleTime);
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
            var d = Math.Abs(eEvent.StartSample - CurrentSampleTime);
            if ( d <= dis)
            {
                dis = d;
                minDisIndex = i;
            }
        }

        return validEvents[minDisIndex];
    }
    public static List<KeyCode> GetAllValidKeyDown()
    {
        List<KeyCode> keys = new List<KeyCode>();
        
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            keys.Add(KeyCode.UpArrow);
        }       
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            keys.Add(KeyCode.RightArrow);
        }      
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            keys.Add(KeyCode.DownArrow);
        }       
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            keys.Add(KeyCode.LeftArrow);
        }
        return keys;
    }
    public static bool HasValidArrowKeyDown()
    {
        return Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow) ||
               Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow);
    }
    // pre requisite: Assume current input is valid
    public static ArrowLevel GetCurrentArrowLevel(bool isDownBeat = true)
    {
        var kEvent = isDownBeat ? GetClosestDownBeatEvent() : GetClosestUpBeatEvent();
        var gap = Math.Abs(kEvent.StartSample - CurrentSampleTime);
        
        if (gap > G.GetThreshold(ArrowLevel.Miss))
        {
            return ArrowLevel.Miss;
        }
        
        if(gap > G.GetThreshold(ArrowLevel.Good))
        {
            return ArrowLevel.Good;
        }
        
        return ArrowLevel.Perfect;
    }

    public static ArrowLevel GetArrowLevel(int targetSampleTime)
    {
        var gap = Math.Abs(targetSampleTime - CurrentSampleTime);
        
        if (gap > G.GetThreshold(ArrowLevel.Miss))
        {
            return ArrowLevel.Miss;
        }
        
        if(gap > G.GetThreshold(ArrowLevel.Good))
        {
            return ArrowLevel.Good;
        }
        
        return ArrowLevel.Perfect;
    }
}
