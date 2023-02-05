using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager singleton;
    public float soundVolume = 1;
    public List<AudioClip> audioClipList;
    
    public enum SoundEffectName
    {
        door,
        fail,
        go,
        harvest,
        hoeing,
        insecticide,
        miss,
        move,
        plantSeed,
        playerHoar,
        ready,
        success,
        water,
    }

    public Dictionary<SoundEffectName, AudioSource> sfxToAudioSources = new Dictionary<SoundEffectName, AudioSource>();

    private void Awake()
    {
        singleton = this;
    }

    public void PlaySFX(SoundEffectName sfxName)
    {
        if (!sfxToAudioSources.ContainsKey(sfxName))
        {
            var audioSrc = transform.AddComponent<AudioSource>();
            sfxToAudioSources[sfxName] = audioSrc;
        }
        var sfx = sfxToAudioSources[sfxName];
        sfx.clip = audioClipList[(int) sfxName];
        sfx.volume = soundVolume;
        sfx.Play();
    }
}
