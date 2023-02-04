using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using SonicBloom.Koreo;
using UnityEngine;

public class FarmingStateMachine : SerializedMonoBehaviour
{
    public Dictionary<PlayerFarmAction, IState> StatesDictionary;
    private PlayerFarmAction curAction = PlayerFarmAction.NoActionNeeded;
    private bool active;
    private bool finished = true;
    private void Awake()
    {
        GameEvents.OnReachedFarmTile += (tile, action) =>
        {
            GameEvents.currentTile = tile;
            curAction = action;
            SwitchTo(action);
        };
        active = false;
        finished = true;
    }
    
    private void Update()
    {
        if (finished)
        {
            if (K.GetAllValidKeyDown().Count > 0)
            {
                active = true;
            }
        
            if (active)
            {
                if (curAction != PlayerFarmAction.NoActionNeeded)
                {
                    StatesDictionary[curAction].UpdateState();
                }
            }
        }
    }
    
    public void SwitchTo(PlayerFarmAction action)
    {
        if (curAction != PlayerFarmAction.NoActionNeeded)
        {
            StatesDictionary[curAction].Reset();
            StatesDictionary[curAction].Exit();
        }

        curAction = action;
        
        if (curAction != PlayerFarmAction.NoActionNeeded)
        {
            G.Indicator.SwitchTo(curAction);
            StatesDictionary[curAction].Enter();
        }
    }
    
    public void Fail()
    {
        G.Indicator.Present(ActionLevel.Miss);
        G.Indicator.UpdateState(ArrowState.Miss);
        active = false;
        StartCoroutine(FailCor());
    }

    public void Success(ActionLevel level)
    {
        active = false;
        G.Indicator.Present(level);
        StartCoroutine(SuccessCor(level));
    }

    IEnumerator FailCor()
    {
        GameManager.singleton.sharedContext.player.SwitchToAnimState(PlayerAnimName.Sad);
        finished = false;
        var sec = K.BeatsPerMinute;
        yield return new WaitForSeconds(60/(float)K.BeatsPerMinute);
        GameManager.singleton.sharedContext.player.SwitchToAnimState(PlayerAnimName.Idle);
        finished = true;
        Reset();
    }  
    
    IEnumerator SuccessCor(ActionLevel level)
    {
        G.Indicator.Close();
        GameManager.singleton.sharedContext.player.SwitchToAnimState(PlayerAnimName.Idle);
        GameEvents.OnFarmActionDone.Invoke(GameEvents.currentTile, curAction, level);
        finished = true;
        Reset();
        yield return null;
    }

    public void Reset()
    {
        if (curAction != PlayerFarmAction.NoActionNeeded)
        {
            StatesDictionary[curAction].Reset();
        }
        G.Indicator.Reset();
    }
}
