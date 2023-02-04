using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Fertilize : MonoBehaviour, IIndicator
{
    public GameObject UI;
    private bool firstPassed;
    public GameObject firstArrow;
    public GameObject secondArrow;
    
    public void Init()
    {
        UI.SetActive(true);
        Reset();
    }

    public void UpdateState(ArrowState state)
    {
        if (state == ArrowState.Miss)
        {
            if (secondArrow.GetComponentInChildren<IUIThumbnail<ArrowState>>() is { } sTN)
            {
                sTN.FillWith(state);
            }    
            if (!firstPassed)
            {
                if (firstArrow.GetComponentInChildren<IUIThumbnail<ArrowState>>() is { } fTN)
                {
                    fTN.FillWith(state);
                } 
            }
        }
        else
        {
            if (firstPassed)
            {
                if (secondArrow.GetComponentInChildren<IUIThumbnail<ArrowState>>() is { } s)
                {
                    s.FillWith(state);
                }   
            }
            else
            {
                if (firstArrow.GetComponentInChildren<IUIThumbnail<ArrowState>>() is { } s)
                {
                    s.FillWith(state);
                }

                firstPassed = true;
            }
        }
    }

    public void Exit()
    {
        UI.SetActive(false);
    }

    public void Reset()
    {
        firstPassed = false;
        
        if (firstArrow.GetComponentInChildren<IUIThumbnail<ArrowState>>() is { } s)
        {
            s.FillWith(ArrowState.Normal);
        }      
        
        if (secondArrow.GetComponentInChildren<IUIThumbnail<ArrowState>>() is { } tn)
        {
            tn.FillWith(ArrowState.Normal);
        }
    }
}
