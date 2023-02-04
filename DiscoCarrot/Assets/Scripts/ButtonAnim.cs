using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnim : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Tweener tween;
    private Button a;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (tween != null) tween.Complete();
        transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f).From(Vector3.one).SetEase(Ease.OutBack);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (tween != null) tween.Complete();
        transform.DOScale(Vector3.one, 0.2f).From(new Vector3(0.9f, 0.9f, 0.9f)).SetEase(Ease.OutBack);
    }
}
