using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Fertilize : MonoBehaviour, IIndicator
{
    private bool firstPassed;
    public GameObject firstArrow;
    public GameObject secondArrow;
    
    public void Init()
    {Reset();
    }

    public void Pass(ArrowState state)
    {
        if (state == ArrowState.Miss)
        {
            if (secondArrow.GetComponent<IUIThumbnail<ArrowState>>() is { } sTN)
            {
                sTN.FillWith(state);
            }    
            if (!firstPassed)
            {
                if (firstArrow.GetComponent<IUIThumbnail<ArrowState>>() is { } fTN)
                {
                    fTN.FillWith(state);
                } 
            }
        }
        else
        {
            if (firstPassed)
            {
                if (secondArrow.GetComponent<IUIThumbnail<ArrowState>>() is { } s)
                {
                    s.FillWith(state);
                }   
            }
            else
            {
                if (firstArrow.GetComponent<IUIThumbnail<ArrowState>>() is { } s)
                {
                    s.FillWith(state);
                }       
            }
        }
    }

    public void Exit()
    {
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
