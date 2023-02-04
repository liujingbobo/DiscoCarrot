using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class I_Water : SerializedMonoBehaviour, IIndicator
{
    [SerializeField] public Dictionary<ArrowState, Sprite> Dic;
    public Image BackGround;
    public RectTransform Mask;
    private Tweener tween;
    
    public void Init()
    {
        Reset();
    }

    public void Pass(ArrowState state)
    {
        if (tween == null)
        {
            if (state == ArrowState.Miss)
            {
                BackGround.sprite = Dic[state];
                Mask.anchorMin = new Vector2(1, Mask.anchorMin.y);
            }
            var duration = K.SampleTimeToTime((int) K.SamplePerBeat * 2);
            float angle = 0;
            tween = DOTween.To(() => Mask.anchorMin.x, x => Mask.anchorMin = new Vector2(x, Mask.anchorMin.y), 1, duration);
        }
        else
        {
            BackGround.sprite = Dic[state];
            Mask.anchorMin = new Vector2(1, Mask.anchorMin.y);
            tween.Complete();
            tween = null;
        }
    }

    public void Exit()
    {
    }

    public void Reset()
    {
        if (tween != null) tween.Complete();
        Mask.anchorMin = new Vector2(0, Mask.anchorMin.y);
        BackGround.sprite = Dic[ArrowState.Good];
        tween = null;
    }
}
