using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Debug : MonoBehaviour, IIndicator
{
    public GameObject clockwiseUI;
    public GameObject counterClockwiseUI;

    public List<GameObject> clockwiseArrows;
    public List<GameObject> counterClockwiseArrows;

    public int process;

    public void Init()
    {
        Reset();

    }

    public void Pass(ArrowState state)
    {
        var ui = GameEvents.isClockWise ? clockwiseUI : counterClockwiseUI;
        
        List<GameObject> arrows = GameEvents.isClockWise ? clockwiseArrows : counterClockwiseArrows;
        
        if (state == ArrowState.Miss)
        {
            for (int i = process; i < arrows.Count; i++)
            {
                if (arrows[i].GetComponentInChildren<IUIThumbnail<ArrowState>>() is { } tn)
                {
                    tn.FillWith(ArrowState.Miss);
                }
            }
        }
        else
        {
            if (arrows[process].GetComponentInChildren<IUIThumbnail<ArrowState>>() is { } tn)
            {
                tn.FillWith(ArrowState.Miss);
            }
            if (process + 1 != arrows.Count) process++;
        }
    }

    public void Exit()
    {
    }

    public void Reset()
    {
        process = 0;
        clockwiseArrows.ForEach(_ =>
        {
            if (_.GetComponentInChildren<IUIThumbnail<ArrowState>>() is { } tn)
            {
                tn.FillWith(ArrowState.Normal);
            }
        });        
        
        counterClockwiseArrows.ForEach(_ =>
        {
            if (_.GetComponentInChildren<IUIThumbnail<ArrowState>>() is { } tn)
            {
                tn.FillWith(ArrowState.Normal);
            }
        });
        clockwiseUI.SetActive(GameEvents.isClockWise);
        counterClockwiseUI.SetActive(!GameEvents.isClockWise);
    }
}
