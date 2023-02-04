using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UI_Thumbnail_Arrow : SerializedMonoBehaviour, IUIThumbnail<ArrowState>
{
    [SerializeField] public Dictionary<ArrowState, Sprite> Dic;

    public Image targetImage;
    private Tweener tween;
    public void FillWith(ArrowState target, params object[] extraInfos)
    {
        targetImage.sprite = Dic[target];
        if(tween != null)tween.Complete();
        if (target == ArrowState.Miss)
        {            
            tween = targetImage.transform.DOPunchScale(-new Vector3(0.5f,0.5f,0.5f), 0.2f);
        }else if (target == ArrowState.Normal)
        {
            tween = targetImage.transform.DOScale( new Vector3(1, 1, 1), 0.3f).From(new Vector3(0.8f, 0.8f, 0.8f)).SetEase(Ease.OutBack);
        }
        else
        {
            tween = targetImage.transform.DOScale( new Vector3(0.8f, 0.8f, 0.8f), 0.3f).From(new Vector3(1, 1, 1)).SetEase(Ease.OutBack);
        }
    }
}
