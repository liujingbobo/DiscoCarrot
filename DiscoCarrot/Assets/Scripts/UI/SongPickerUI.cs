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

    private void OnEnable()
    {
        K.musicPlayer.LoadSong(MenuMusic);
        K.musicPlayer.Play();
    }

    public void PickMagic()
    {
        //K.musicPlayer.Stop();

        //K.musicPlayer.LoadSong(MagicKoreo);
        GameManager.singleton.sharedContext.runTimeValues.pickedSongKore = MagicKoreo;
        GameManager.singleton.stateMachine.SwitchToState(GameManager.GameLoopState.GameCutScene);
    }

    public void PickTLT()
    {
        //K.musicPlayer.Stop();
        //K.musicPlayer.LoadSong(TLTKoreo);
        GameManager.singleton.sharedContext.runTimeValues.pickedSongKore = TLTKoreo;
        GameManager.singleton.stateMachine.SwitchToState(GameManager.GameLoopState.GameCutScene);
    }

    public void PickBL()
    {
        //K.musicPlayer.Stop();
        //K.musicPlayer.LoadSong(BLKoreo);
        GameManager.singleton.sharedContext.runTimeValues.pickedSongKore = BLKoreo;
        GameManager.singleton.stateMachine.SwitchToState(GameManager.GameLoopState.GameCutScene);
    }
}
