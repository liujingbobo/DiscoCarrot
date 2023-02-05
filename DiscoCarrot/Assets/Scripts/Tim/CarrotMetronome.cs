using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CarrotMetronome : MonoBehaviour
{
    public Image CarrotImage;
    public Vector3 CarrotPunchScale = Vector3.one;
    public RectTransform leftStartPos;
    public RectTransform rightStartPos;
    public RectTransform centerEndPos;
    public List<RectTransform> DownBeats;
    public List<RectTransform> UpBeats;
    public RectTransform metronomePanel;

    public RectTransform hidePosRectTransform;
    public RectTransform showPosRectTransform;
    //config values
    public int maxNotesOnScreen = 4;
    public float reachCenterTime = 1;
    public float secondsPerBeat = 1;


    private Coroutine DownBeatCoroutine;
    private Coroutine UpBeatCoroutine;
    private Tweener pumpTweener;
    
    private void Start()
    {
        RegisterBeatEvent();
    }

    public void ShowMetronomePanel()
    {
        metronomePanel.DOMove(showPosRectTransform.position, 2).SetEase(Ease.Linear);
    }
    public void HideMetronomePanel()
    {
        metronomePanel.DOMove(hidePosRectTransform.position, 0.5f).SetEase(Ease.Linear);

    }
    public void RegisterBeatEvent()
    {
        GameEvents.OnDownBeat += OnDownBeat;
        GameEvents.OnUpBeat += OnUpBeat;
    }

    private void OnDownBeat()
    {
        SendDownBeat();
        if(pumpTweener != null) pumpTweener.Kill();
        CarrotImage.transform.localScale = Vector3.one;
        pumpTweener = CarrotImage.transform.DOPunchScale(CarrotPunchScale, 0.1f);
    }

    private void OnUpBeat()
    {
        SendUpBeat();
    }

    [ContextMenu("test")]
    public void Test()
    {
        ConfigMetronome(123);
    }
    
    public void ConfigMetronome(float bpm)
    {
        secondsPerBeat = 60 / bpm;
        reachCenterTime = secondsPerBeat * maxNotesOnScreen;
    }
    
    public void Reset()
    {
        metronomePanel.position = hidePosRectTransform.position;
        foreach (var note in DownBeats)
        {
            note.gameObject.SetActive(false);
        }
        foreach (var note in UpBeats)
        {
            note.gameObject.SetActive(false);
        }
    }

    [ContextMenu("testDownBeat")]
    public void SendDownBeat()
    {
        //search for inactive beat
        RectTransform leftNote = null;
        RectTransform rightNote = null;
        foreach (var note in DownBeats)
        {
            if (note.gameObject.activeSelf == false)
            {
                if(leftNote == null) leftNote = note;
                else if (rightNote == null) rightNote = note;
            }
        }

        if (leftNote == null || rightNote == null)
        {
            Debug.LogError($"Not Enough Down NOTES!!!!");
            return;
        }
        
        //set to default state
        leftNote.gameObject.SetActive(true);
        rightNote.gameObject.SetActive(true);
        leftNote.position = leftStartPos.position;
        rightNote.position = rightStartPos.position;
        leftNote.localScale = Vector3.one;
        rightNote.localScale = Vector3.one;
        var leftNoteImage = leftNote.GetComponent<Image>();
        var rightNoteImage = rightNote.GetComponent<Image>();
        leftNoteImage.color = Color.white;
        rightNoteImage.color = Color.white;
        
        //move and at last fade
        var leftMove = leftNote.DOMoveX(centerEndPos.position.x, reachCenterTime).SetEase(Ease.Linear);
        var leftScale = leftNote.DOScale(1.5f, 0.2f);
        var leftFade = leftNoteImage.DOFade(0, 0.2f);
        DOTween.Sequence().Append(leftMove).Append(leftScale).Join(leftFade).AppendCallback(() => { 
                leftNote.gameObject.SetActive(false);
            });
        var rightMove = rightNote.DOMoveX(centerEndPos.position.x, reachCenterTime).SetEase(Ease.Linear);
        var rightScale = rightNote.DOScale(1.5f, 0.2f);
        var rightFade = rightNoteImage.DOFade(0, 0.2f);
        DOTween.Sequence().Append(rightMove).Append(rightScale).Join(rightFade).AppendCallback(() => { 
            rightNote.gameObject.SetActive(false);
        });
        
    }
    
    [ContextMenu("testUpBeat")]
    public void SendUpBeat()
    {
        //search for inactive beat
        RectTransform leftNote = null;
        RectTransform rightNote = null;
        foreach (var note in UpBeats)
        {
            if (note.gameObject.activeSelf == false)
            {
                if(leftNote == null) leftNote = note;
                else if (rightNote == null) rightNote = note;
            }
        }

        if (leftNote == null || rightNote == null)
        {
            Debug.LogError($"Not Enough Up NOTES!!!!");
            return;
        }
        
        //set to default state
        leftNote.gameObject.SetActive(true);
        rightNote.gameObject.SetActive(true);
        leftNote.localScale = Vector3.one * 0.7f;
        rightNote.localScale = Vector3.one * 0.7f;
        leftNote.position = leftStartPos.position;
        rightNote.position = rightStartPos.position;
        var leftNoteImage = leftNote.GetComponent<Image>();
        var rightNoteImage = rightNote.GetComponent<Image>();
        leftNoteImage.color = Color.white;
        rightNoteImage.color = Color.white;
        
        //move and at last fade
        var leftMove = leftNote.DOMoveX(centerEndPos.position.x, reachCenterTime).SetEase(Ease.Linear);
        var leftScale = leftNote.DOScale(1f, 0.2f);
        var leftFade = leftNoteImage.DOFade(0, 0.2f);
        DOTween.Sequence().Append(leftMove).Append(leftScale).Join(leftFade).AppendCallback(() => { 
            leftNote.gameObject.SetActive(false);
        });
        var rightMove = rightNote.DOMoveX(centerEndPos.position.x, reachCenterTime).SetEase(Ease.Linear);
        var rightScale = rightNote.DOScale(1f, 0.2f);
        var rightFade = rightNoteImage.DOFade(0, 0.2f);
        DOTween.Sequence().Append(rightMove).Append(rightScale).Join(rightFade).AppendCallback(() => { 
            rightNote.gameObject.SetActive(false);
        });
        
    }
}
