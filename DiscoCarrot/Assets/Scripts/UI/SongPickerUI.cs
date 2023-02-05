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
        source.volume = 0.8f;
        K.musicPlayer.Stop();
        GameManager.singleton.sharedContext.pickedSongKore = MagicKoreo;
        GameManager.singleton.stateMachine.SwitchToState(GameManager.GameLoopState.GameCutScene);
    }

    public void PickTLT()
    {
        source.volume = 0.8f;
        K.musicPlayer.Stop();
        GameManager.singleton.sharedContext.pickedSongKore = TLTKoreo;
        GameManager.singleton.stateMachine.SwitchToState(GameManager.GameLoopState.GameCutScene);
    }

    public void PickBL()
    {
        source.volume = 0.8f;
        K.musicPlayer.Stop();
        GameManager.singleton.sharedContext.pickedSongKore = BLKoreo;
        GameManager.singleton.stateMachine.SwitchToState(GameManager.GameLoopState.GameCutScene);
    }
}
