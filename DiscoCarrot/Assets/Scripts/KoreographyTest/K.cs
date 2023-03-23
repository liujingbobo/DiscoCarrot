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
    public static int SampleRate => koreographer.GetMusicSampleRate(); // Sample/Seconds
    public static float SamplePerBeat => G.Settings.SamplesPerBeatForSong[currentClip];
    public static double BeatsPerMinute => koreographer.GetMusicBPM();
    public static float SamplePerSecond => SamplePerBeat * (float)BeatsPerMinute / 60;
    public static float SampleTimeToTime(int sampleTime)
    {
        return (float)sampleTime / SampleRate;
    }
    public static KoreographyEvent GetClosestDownBeatEvent()
    {
        return GetClosestEvent(G.Settings.DownBeatEvent);
    }
    public static KoreographyEvent GetClosestUpBeatEvent()
    {
        return GetClosestEvent(G.Settings.UpBeatEvent);
    }

    public static void LoadSong(Koreography kore)
    {
        musicPlayer.LoadSong(kore);
    }
    
    public static KoreographyEvent GetClosestEvent(string eventName)
    {
        // var allEvents = koreographer.GetAllEvents(CurrentClip, eventName);
        //
        // // Make sure the events are sorted by StartSample.
        // allEvents.Sort((e1, e2) => e1.StartSample.CompareTo(e2.StartSample));
        //
        // int index = BinarySearchClosestEvent(allEvents, CurrentSampleTime);

        return default;
    }
    
    
    private static int BinarySearchClosestEvent(List<KoreographyEvent> events, int targetSampleTime)
    {
        int left = 0;
        int right = events.Count - 1;

        while (left < right)
        {
            int mid = left + (right - left) / 2;

            if (events[mid].StartSample < targetSampleTime)
            {
                left = mid + 1;
            }
            else
            {
                right = mid;
            }
        }

        if (left == 0)
        {
            return 0;
        }

        int closestLeft = Math.Abs(events[left - 1].StartSample - targetSampleTime);
        int closestRight = Math.Abs(events[left].StartSample - targetSampleTime);

        return closestLeft <= closestRight ? left - 1 : left;
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
    public static List<KeyCode> GetAllValidKeyUp()
    {
        List<KeyCode> keys = new List<KeyCode>();
        
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            keys.Add(KeyCode.UpArrow);
        }       
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            keys.Add(KeyCode.RightArrow);
        }      
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            keys.Add(KeyCode.DownArrow);
        }       
        if (Input.GetKeyUp(KeyCode.LeftArrow))
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
    public static PressLevel GetCurrentArrowLevel(bool isDownBeat = true)
    {
        var kEvent = isDownBeat ? GetClosestDownBeatEvent() : GetClosestUpBeatEvent();
        var gapSample = Math.Abs(kEvent.StartSample - CurrentSampleTime);
        var gapTime = SampleTimeToTime(gapSample);
        if (gapTime > G.GetThreshold(PressLevel.Miss))
        {
            return PressLevel.Miss;
        }
        
        if(gapTime > G.GetThreshold(PressLevel.Good))
        {
            return PressLevel.Good;
        }
        
        return PressLevel.Perfect;
    }
    public static PressLevel GetArrowLevel(int targetSampleTime)
    {
        var gapSample = Math.Abs(targetSampleTime - CurrentSampleTime);
        var gapTime = SampleTimeToTime(gapSample);

        if (gapTime > G.GetThreshold(PressLevel.Miss))
        {
            return PressLevel.Miss;
        }
        
        if(gapTime > G.GetThreshold(PressLevel.Good))
        {
            return PressLevel.Good;
        }
        
        return PressLevel.Perfect;
    }
    public static bool GetOnlyKeyDown(KeyCode key)
    {
        return Input.GetKeyDown(key) && GetAllValidKeyDown().Count == 1;
    }

    public static int GetMaxSampleTime(KoreographyEvent kEvent, float BeatAway)
    {
        var temp = G.GetThreshold(PressLevel.Miss) * SamplePerSecond;
        
        return (int)(kEvent.EndSample + BeatAway * SamplePerBeat + temp);
    }

    public static ArrowState ToArrowState(this PressLevel level)
    {
        return level switch
        {
            PressLevel.Perfect => ArrowState.Perfect,
            PressLevel.Good => ArrowState.Good,
            PressLevel.Miss => ArrowState.Miss,
            _ => ArrowState.Normal
        };
    }
}
