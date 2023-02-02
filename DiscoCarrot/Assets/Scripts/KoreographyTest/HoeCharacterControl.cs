using System;
using System.Collections;
using System.Collections.Generic;
using SonicBloom.Koreo;
using UnityEngine;

public class HoeCharacterControl : MonoBehaviour
{
    public float threshold;
    [EventID] public string downBeat;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            var sampleThreshold = threshold * Koreographer.Instance.GetMusicSampleRate();
        }
    }
}
