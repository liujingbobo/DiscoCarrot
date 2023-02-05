using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;
using UnityEngine;
using UnityEngine.UI;

public class SongPickerUI : SerializedMonoBehaviour
{
    public Koreography MagicKoreo;
    public Koreography TLTKoreo;
    public Koreography BLKoreo;

    public Koreography MenuMusic;

    public AudioSource source;

    private void OnEnable()
    {
        if (!source.isPlaying)
        {
            source.Play();
        }
        
    }

    public void PickMagic()
    {
        source.Stop();
        K.musicPlayer.Stop();
        K.musicPlayer.LoadSong(MagicKoreo);
        GameManager.singleton.stateMachine.SwitchToState(GameManager.GameLoopState.GameReadyStart);
    }

    public void PickTLT()
    {
        source.Stop();
        K.musicPlayer.Stop();
        K.musicPlayer.LoadSong(TLTKoreo);
        GameManager.singleton.stateMachine.SwitchToState(GameManager.GameLoopState.GameReadyStart);
    }

    public void PickBL()
    {
        source.Stop();
        K.musicPlayer.Stop();
        K.musicPlayer.LoadSong(BLKoreo);
        GameManager.singleton.stateMachine.SwitchToState(GameManager.GameLoopState.GameReadyStart);
    }
}
