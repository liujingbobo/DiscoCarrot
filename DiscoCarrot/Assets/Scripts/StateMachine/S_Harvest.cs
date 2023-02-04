using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Harvest : MonoBehaviour, IState
{
    private int phase;
    
    public void Enter()
    {
        G.Indicator.SwitchTo(PlayerFarmAction.HarvestPlant);
        Reset();
    }

    public void Exit()
    {
    }

    public void Reset()
    {
        phase = 0;
    }

    public void UpdateState()
    {
        var isEven = phase % 2 == 0;

        if (isEven)
        {
            var allKeys = K.GetAllValidKeyDown();

            if (allKeys.Count > 0)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow) && allKeys.Count == 1)
                {
                    var level = K.GetCurrentArrowLevel(false);

                    if (level == ArrowLevel.Miss)
                    {
                        // failed
                    }
                    else
                    {
                        phase++;
                        // Update UI
                    }
                }
                else
                {
                    // Failed
                }
            }
        }
        else
        {
            var allKeys = K.GetAllValidKeyDown();

            if (allKeys.Count > 0)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow) && allKeys.Count == 1)
                {
                    var level = K.GetCurrentArrowLevel(false);

                    if (level == ArrowLevel.Miss)
                    {
                        // failed
                    }
                    else
                    {
                        if (phase == 7)
                        {
                            // success
                        }
                        else
                        {
                            phase++;
                            // Update UI
                        }
                    }
                }
                else
                {
                    // Failed
                }
            }
        }
    }
}
