using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CarrotMetronome : MonoBehaviour
{
    public RectTransform leftStartPos;
    public RectTransform rightStartPos;
    public RectTransform centerEndPos;
    public List<RectTransform> DownBeats;
    public List<RectTransform> UpBeats;
    public RectTransform notesContainer;

    //config values
    public int maxNotesOnScreen = 4;
    public float reachCenterTime = 1;
    public float secondsPerBeat = 1;


    private Coroutine DownBeatCoroutine;
    private Coroutine UpBeatCoroutine;

    public void RegisterBeatEvent()
    {
    }
    
    [ContextMenu("test")]
    public void Test()
    {
        /*ConfigMetronome(123);
        StartShowingNotes();*/
    }
    
    public void ConfigMetronome(float bpm)
    {
        secondsPerBeat = 60 / bpm;
        reachCenterTime = secondsPerBeat * maxNotesOnScreen;
    }
    
    public void StartShowingNotes()
    {
        if(DownBeatCoroutine != null) StopCoroutine(DownBeatCoroutine);
        DownBeatCoroutine = StartCoroutine(ShowDownBeatCoroutine());
        
        Debug.Log(secondsPerBeat / 2);
        DOTween.Sequence().AppendInterval(secondsPerBeat / 2).AppendCallback(() =>
        {
            if(UpBeatCoroutine != null) StopCoroutine(UpBeatCoroutine);
            UpBeatCoroutine = StartCoroutine(ShowUpBeatCoroutine());
        });

    }
    
    [ContextMenu("stop")]
    public void StopShowingNotes()
    {
        if(DownBeatCoroutine != null) StopCoroutine(DownBeatCoroutine);
        if(UpBeatCoroutine != null) StopCoroutine(UpBeatCoroutine);
    }

    IEnumerator ShowDownBeatCoroutine()
    {
        while (true)
        {
            SendDownBeat();
            yield return new WaitForSeconds(secondsPerBeat);
        }
    }
    IEnumerator ShowUpBeatCoroutine()
    {
        while (true)
        {
            SendUpBeat();
            yield return new WaitForSeconds(secondsPerBeat);
        }
    }
    public void Reset()
    {
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
        leftNote.localScale = Vector3.one;
        rightNote.localScale = Vector3.one;
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
