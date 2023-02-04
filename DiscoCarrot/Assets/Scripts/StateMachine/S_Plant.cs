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
        var allValidKeyDown = K.GetAllValidKeyDown();

        if (allValidKeyDown.Count > 0)
        {
            if (allValidKeyDown.Contains(KeyCode.DownArrow) && allValidKeyDown.Count == 1)
            {
                var level = K.GetCurrentArrowLevel();
            
                if ( level == PressLevel.Miss)
                {
                    // Failed
                    G.StateMachine.Fail();
                }
                else
                {
                    // Success
                    // GameEvents.OnFarmActionDone.Invoke();
                    G.Indicator.UpdateState(ArrowState.Perfect);
                    G.StateMachine.Success(ActionLevel.Perfect);
                }
            }else
            {
                // Failed
                G.StateMachine.Fail();
            }
        }
    }
}
