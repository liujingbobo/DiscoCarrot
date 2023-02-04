using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Plant : MonoBehaviour, IState
{
    public void Enter()
    {
        G.Indicator.SwitchTo(PlayerFarmAction.PlantSeed);
        Reset();
    }

    public void Exit()
    {
    }

    public void Reset()
    {
    }

    public void UpdateState()
    {
    }

    private void Update()
    {
        var allValidKeyDown = K.GetAllValidKeyDown();

        if (allValidKeyDown.Count > 0)
        {
            if (allValidKeyDown.Contains(KeyCode.DownArrow) && allValidKeyDown.Count == 1)
            {
                var level = K.GetCurrentArrowLevel();
            
                if ( level == ArrowLevel.Miss)
                {
                    // Failed
                }
                else
                {
                    // GameEvents.OnFarmActionDone.Invoke();
                }
            }else
            {
                // Failed
            }
        }

    }
}
