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

    // public void Fail()
    // {
    //     active = false;
    //     // GameManager.singleton.k
    //     // play idle
    // }

    public void Fail()
    {
        G.Indicator.UpdateState(ArrowState.Miss);
        active = false;
        StartCoroutine(FailCor());
        // switch fail anim
        GameManager.singleton.sharedContext.player.SwitchToAnimState(PlayerAnimName.Sad);
        // ---
        // switch to idle
        
    }

    public void Success(ActionLevel level)
    {
        active = false;
        StartCoroutine(SuccessCor());
    }

    IEnumerator FailCor()
    {
        GameManager.singleton.sharedContext.player.SwitchToAnimState(PlayerAnimName.Sad);
        finished = false;
        yield return new WaitForSeconds(K.SamplePerBeat * K.SamplePerSecond);
        GameManager.singleton.sharedContext.player.SwitchToAnimState(PlayerAnimName.Idle);
        finished = true;
        Reset();
    }  
    
    IEnumerator SuccessCor()
    {
        GameManager.singleton.sharedContext.player.SwitchToAnimState(PlayerAnimName.Sad);
        finished = false;
        yield return new WaitForSeconds(K.SamplePerBeat * K.SamplePerSecond);
        GameManager.singleton.sharedContext.player.SwitchToAnimState(PlayerAnimName.Idle);
        
        // TODO: 
        GameEvents.OnFarmActionDone.Invoke(GameEvents.currentTile, curAction, ActionLevel.Perfect);
        finished = true;
        Reset();
    }

    public void Reset()
    {
        if (curAction != PlayerFarmAction.NoActionNeeded)
        {
            StatesDictionary[curAction].Reset();
        }
        G.Indicator.Reset();
    }

    public void SetCurrentTile(string action)
    {
        PlayerFarmAction result = 0;
        Enum.TryParse(action, out  result);
        curAction = result;
        SwitchTo(result);
    }
}
