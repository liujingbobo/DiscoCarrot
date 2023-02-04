using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager singleton;
    public List<AudioClip> audioClipList;
    
    public enum SoundEffectName
    {
        
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
        sfx.Play();
    }
}
