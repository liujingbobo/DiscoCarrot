using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Plant : MonoBehaviour, IState
{
    private bool block = false;
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
        if (block) return;
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
                    var l = level switch
                    {
                        PressLevel.Perfect => ActionLevel.Perfect,
                        PressLevel.Good => ActionLevel.Good
                    };
                    G.Indicator.Present(l);
                    block = true;
                    StartCoroutine(Success(l));
                }
            }else
            {
                // Failed
                G.StateMachine.Fail();
            }
        }
    }
    
    IEnumerator Success(ActionLevel level)
    {
        yield return new WaitForSeconds(K.SampleTimeToTime((int) K.SamplePerBeat));
        G.StateMachine.Success(level);
        block = false;
    }
}
