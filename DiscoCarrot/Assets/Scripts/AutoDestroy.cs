using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public CanvasGroup canvas;
    
    public void OnEnable()
    {
        StartCoroutine(SelfDestroy());
    }

    IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(.4f);
        yield return canvas.DOFade(0, 1f);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
