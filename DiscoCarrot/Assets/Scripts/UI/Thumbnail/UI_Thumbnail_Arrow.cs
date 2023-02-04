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

    public void FillWith(ArrowState target, params object[] extraInfos)
    {
        targetImage.sprite = Dic[target];
        targetImage.transform.DOPunchScale(Vector3.one, 0.2f);
    }
}
