using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Plant : MonoBehaviour, IIndicator
{
    public GameObject target;
    
    public void Init()
    {
        Reset();
    }

    public void Pass(ArrowState state)
    {
        if (target.GetComponentInChildren<IUIThumbnail<ArrowState>>() is { } tn)
        {
            tn.FillWith(state);
        }
    }

    public void Exit()
    {
    }

    public void Reset()
    {
        if (target.GetComponentInChildren<IUIThumbnail<ArrowState>>() is { } tn)
        {
            tn.FillWith(ArrowState.Normal);
        }
    }
}
