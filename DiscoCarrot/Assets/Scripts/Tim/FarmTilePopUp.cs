using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FarmTilePopUp : MonoBehaviour
{
    public float popDuration;
    public float closeDuration;
    public Transform popupRoot;
    public SpriteRenderer spriteRenderer;
    public Sprite waterSprite;
    public Sprite fertilizerSprite;
    public Sprite bugSprite;

    private Tweener openTweener;
    private Tweener closeTweener;
    private int liveBeatCount;
    
    public bool isPoppingUp;
    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        popupRoot.localScale = Vector3.zero;
    }
    
    [ContextMenu("TestPopUp")]
    public void TestPopUp()
    {
        StartPopUp(FarmTile.FarmTileEventFlag.NeedDebug,16);
    }
    [ContextMenu("TestClosePopUp")]
    public void TestClosePopUp()
    {
        ClosePopUp();
    }
    
    public void StartPopUp(FarmTile.FarmTileEventFlag eventFlag, int liveBeatCount)
    {
        if (eventFlag == FarmTile.FarmTileEventFlag.NeedDebug) spriteRenderer.sprite = bugSprite;
        else if (eventFlag == FarmTile.FarmTileEventFlag.NeedWater) spriteRenderer.sprite = waterSprite;
        else if (eventFlag == FarmTile.FarmTileEventFlag.NeedFertilize) spriteRenderer.sprite = fertilizerSprite;
        
        isPoppingUp = true;

        popupRoot.localScale = Vector3.zero;
        if(openTweener != null) {openTweener.Kill();}
        if(closeTweener != null) {closeTweener.Kill();}
        openTweener = popupRoot.DOScale(1, popDuration).SetEase(Ease.InOutElastic);
    }

    public void ClosePopUp()
    {
        if(openTweener != null) {openTweener.Kill();}
        if(closeTweener != null) {closeTweener.Kill();}

        isPoppingUp = false;
        popupRoot.localScale = Vector3.one;
        closeTweener = popupRoot.DOScale(0, closeDuration).SetEase(Ease.InBack);
    }
}
