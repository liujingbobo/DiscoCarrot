using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Plant : MonoBehaviour, IIndicator
{
    public GameObject UI;
    public GameObject target;
    
    public void Init()
    {
        Reset();
        UI.SetActive(true);
    }

    public void UpdateState(ArrowState state)
    {
        if (target.GetComponentInChildren<IUIThumbnail<ArrowState>>() is { } tn)
        {
            tn.FillWith(state);
        }
    }

    public void Exit()
    {
        UI.SetActive(false);
            
    }

    public void Reset()
    {
        if (target.GetComponentInChildren<IUIThumbnail<ArrowState>>() is { } tn)
        {
            tn.FillWith(ArrowState.Normal);
        }
    }
}
