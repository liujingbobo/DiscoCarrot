using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SongPickerUI : SerializedMonoBehaviour
{
    public struct SongData
    {
        public int BPM;
        public Sprite Cover;
        public string SongName;
        public string Singer;
        public Difficulty difficulty;
    }

    public List<SongData> Datas;

    public List<GameObject> Thumbnails;

    public int pivot;

    public Text BPMText;
    public Text SongName;
    public Text Singer;
    

    public Dictionary<Difficulty, GameObject> DifficultyStars;

    public SongData CurrentSongData => Datas[pivot];
    
    public void Reset()
    {
        pivot = 0;
        for (int i = 0; i < Thumbnails.Count; i++)
        {
            int index = pivot + i % Thumbnails.Count;
            // if (Thumbnails[index].GetComponentInChildren<IUIThumbnail<SongData>>() is {})
        }
    }

    public void MoveNext()
    {
        pivot++;
        pivot = pivot % Thumbnails.Count;
        Refresh();
    }

    public void MovePrevious()
    {
        pivot--;
        pivot = pivot % Thumbnails.Count;
        Refresh();
    }

    public void Refresh()
    {
        Singer.text = CurrentSongData.Singer;
        BPMText.text = CurrentSongData.BPM.ToString();
        SongName.text = CurrentSongData.SongName;

        DifficultyStars[Difficulty.Normal].SetActive(CurrentSongData.difficulty == Difficulty.Normal);
        DifficultyStars[Difficulty.Hard].SetActive(CurrentSongData.difficulty == Difficulty.Hard);
        DifficultyStars[Difficulty.Hell].SetActive(CurrentSongData.difficulty == Difficulty.Hell);

        
    



    }

    public void Pick()
    {
        GameManager.singleton.stateMachine.SwitchToState(GameManager.GameLoopState.GameRunning);
    }
}
