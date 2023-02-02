using System;
using System.Collections;
using System.Collections.Generic;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;
using UnityEngine;
using UnityEngine.Serialization;

public class ArrowTest : MonoBehaviour
{
    public SimpleMusicPlayer player;
    public Koreographer koreographer;

    public float Speed;

    public GameObject BarPrefab;
    public GameObject Arrow_Up;
    public GameObject Arrow_Right;
    public GameObject Arrow_Down;
    public GameObject Arrow_Left;

    public RectTransform StartPivot;
    public RectTransform EndPivot;

    private int previousSampleTime;
    private float barLength;
    private int gameBarTotalSample;
    private float gamebarTotalTime;
    private string currentClipName;
    
    [EventID] public string bar;
    [EventID] public string Dig_Event;
    [EventID] public string Seed_Event;
    [EventID] public string down;
    [EventID] public string left;

    public Transform parent;
    
    public void Start()
    {
        currentClipName = player.GetCurrentClipName();
        barLength = StartPivot.anchoredPosition.x - EndPivot.anchoredPosition.x;
        gamebarTotalTime = barLength / Speed;
        gameBarTotalSample = (int)(gamebarTotalTime * koreographer.GetMusicSampleRate());
        player.Play();
    }

    public void FixedUpdate()
    {
        var currentSampleTime = koreographer.GetMusicSampleTime();
        var maxTime = currentSampleTime + gameBarTotalSample;
        var barEvents = koreographer.GetAllEventsInRange(currentClipName, bar, previousSampleTime,
            maxTime);
        foreach (var barEvent in barEvents)
        {
            var temp = Instantiate(BarPrefab, parent);
            var length = SampleTimeToLength(barEvent.EndSample - currentSampleTime);
            ((RectTransform)temp.transform).anchoredPosition = new Vector3(EndPivot.anchoredPosition.x + length, EndPivot.anchoredPosition.y, 0);
        }

        var DigEvents = koreographer.GetAllEventsInRange(currentClipName, Dig_Event, previousSampleTime,
            maxTime);
        foreach (var digEvent in DigEvents)
        {
            if (digEvent.HasIntPayload())
            {
                var intValue = digEvent.GetIntValue();
                switch (intValue)
                {
                    case 0:
                        var newArrow = Instantiate(Arrow_Up, parent);
                        var length = SampleTimeToLength(digEvent.EndSample - currentSampleTime);
                        ((RectTransform)newArrow.transform).anchoredPosition = new Vector3(EndPivot.anchoredPosition.x + length, EndPivot.anchoredPosition.y, 0);
                        break;
                    case 1:
                        break;
                    
                }
            }
        }
        previousSampleTime = currentSampleTime  + gameBarTotalSample;
    }

    public float SampleTimeToLength(int sampleTime)
    {
        var temp = Speed * SampleTimeToTime(sampleTime);
        return temp;
    }

    public float SampleTimeToTime(int sampleTime)
    {
        var temp = (float) sampleTime / koreographer.GetMusicSampleRate();
        ;
        return temp;
    }
    
    
    public void Read(KoreographyEventCallbackWithTime a)
    {
        
    }
}
