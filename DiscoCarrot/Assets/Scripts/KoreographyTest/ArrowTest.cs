using System;
using System.Collections;
using System.Collections.Generic;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;
using UnityEngine;

public class ArrowTest : MonoBehaviour
{
    public GameObject ArrowPrefab;
    public SimpleMusicPlayer player;
    [EventID] public string arrow;
    public Koreographer grapher;
    public void Start()
    {
        player.Play();
        var clipName = player.GetCurrentClipName();
        var totalTime = grapher.musicPlaybackController.GetTotalSampleTimeForClip(clipName);
        var temp = grapher.GetAllEventsInRange(clipName, arrow, 0,
            totalTime);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
        }
    }
}
