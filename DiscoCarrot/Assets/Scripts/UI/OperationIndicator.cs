using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperationIndicator : MonoBehaviour
{
    public Dictionary<PlayerFarmAction, IState> dic;
    private PlayerFarmAction curAction = PlayerFarmAction.NoActionNeeded;
    public void SwitchTo(PlayerFarmAction action)
    {
        if (curAction != PlayerFarmAction.NoActionNeeded && action != curAction)
        {
            dic[curAction].Exit();
        }

        curAction = action;
        
        dic[action].Enter();
    }

    public void Refresh()
    {
        if (curAction != PlayerFarmAction.NoActionNeeded)
        {
            dic[curAction].Reset();
        }
    }
}
