using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class I_Harvest : SerializedMonoBehaviour, IIndicator
{
    public GameObject UI;
    public List<IUIThumbnail<ArrowState>> arrows;
    public int process;
    
    public void Init()
    {
        Reset();
        UI.SetActive(true);
    }

    public void UpdateState(ArrowState state)
    {
        if (state == ArrowState.Miss)
        {
            for (int i = process; i < arrows.Count; i++)
            {
                arrows[i].FillWith(ArrowState.Miss);
            }
        }
        else
        {
            arrows[process].FillWith(state);
            if (process + 1 != arrows.Count) process++;
        }
    }

    public void Exit()
    {
        UI.SetActive(false);
    }

    public void Reset()
    {
        process = 0;
        arrows.ForEach(_ =>
        {
            _.FillWith(ArrowState.Normal);
        });
    }
}