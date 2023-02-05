using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using SonicBloom.Koreo;
using UnityEngine;

public class FarmingStateMachine : SerializedMonoBehaviour
{
    public Dictionary<PlayerFarmAction, IState> StatesDictionary;
    private PlayerFarmAction preAction = PlayerFarmAction.NoActionNeeded;
    private FarmTile preTile;
    private bool active;
    private bool finished = true;

    private Player player => GameManager.singleton.sharedContext.player;
    private FarmTile curTile => player.CurTile;
    
    private void Awake()
    {
        active = false;
        finished = true;
        preTile = null;
    }
    
    private void Update()
    {
        if (preTile != curTile)
        {
            preTile = curTile;
            if (preTile == null)
            {
                SwitchTo(PlayerFarmAction.NoActionNeeded);
            }
            else
            {
                SwitchTo(preTile.GetNeededPlayerFarmAction());
            }
        }
        else
        {
            if (curTile != null && curTile.GetNeededPlayerFarmAction() != preAction)
            {
                SwitchTo(curTile.GetNeededPlayerFarmAction());
            }
        }
        
        if (finished)
        {
            if (K.GetAllValidKeyDown().Count > 0)
            {
                active = true;
            }
        
            if (active)
            {
                if (preAction != PlayerFarmAction.NoActionNeeded)
                {
                    StatesDictionary[preAction].UpdateState();
                }
            }
        }
    }
    
    public void SwitchTo(PlayerFarmAction action)
    {
        if (preAction != PlayerFarmAction.NoActionNeeded)
        {
            StatesDictionary[preAction].Reset();
            StatesDictionary[preAction].Exit();
            G.Indicator.Close();
        }

        preAction = action;
        
        if (preAction != PlayerFarmAction.NoActionNeeded)
        {
            G.Indicator.SwitchTo(preAction);
            StatesDictionary[preAction].Enter();
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
        // G.Indicator.Present(level);
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
        GameEvents.OnFarmActionDone.Invoke(curTile, preAction, level);
        finished = true;
        yield return null;
    }

    public void Reset()
    {
        if (preAction != PlayerFarmAction.NoActionNeeded)
        {
            StatesDictionary[preAction].Reset();
        }
        G.Indicator.Reset();
    }
}
