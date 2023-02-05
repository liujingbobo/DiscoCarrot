using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class I_Water : SerializedMonoBehaviour, IIndicator
{
    public GameObject UI;
    public GameObject Bar;
    [SerializeField] public Dictionary<ArrowState, Sprite> Dic;
    public Image BackGround;
    public RectTransform Mask;
    private Tweener tween;
    private Tweener scaleTween;
    public void Init()
    {
        Reset();
        UI.SetActive(true);
    }

    public void UpdateState(ArrowState state)
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
            if(scaleTween != null) scaleTween.Complete();
            scaleTween = Bar.transform.DOScale(new Vector3(0.97f, 0.97f, 0.97f), 0.2f).From(Vector3.one)
                .SetEase(Ease.OutBack);
        }
        else
        {
            if(scaleTween != null) scaleTween.Complete();
            scaleTween = Bar.transform.DOScale(Vector3.one, 0.2f).From(new Vector3(0.97f, 0.97f, 0.97f))
                .SetEase(Ease.OutBack);
            BackGround.sprite = Dic[state];
            Mask.anchorMin = new Vector2(1, Mask.anchorMin.y);
            tween.Complete();
            tween = null;
        }
    }
    
    public void Exit()
    {
        UI.SetActive(false);
    }

    public void Reset()
    {
        if (tween != null) tween.Complete();
        Mask.anchorMin = new Vector2(0, Mask.anchorMin.y);
        BackGround.sprite = Dic[ArrowState.Good];
        tween = null;
    }
}
