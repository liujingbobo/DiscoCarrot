using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class OperationIndicator : SerializedMonoBehaviour
{
    [SerializeField] private Dictionary<PlayerFarmAction, IIndicator> dic;
    private PlayerFarmAction curAction = PlayerFarmAction.NoActionNeeded;

    public Dictionary<ActionLevel, GameObject> Prefabs;
    public Transform Parent;


    public void SwitchTo(PlayerFarmAction action)
    {
        if (curAction != PlayerFarmAction.NoActionNeeded && action != curAction)
        {
            dic[curAction].Exit();
        }

        curAction = action;
        dic[action].Init();
    }
    public void Reset()
    {
        if (curAction != PlayerFarmAction.NoActionNeeded)
        {
            dic[curAction].Exit();
            dic[curAction].Init();
        }
    }
    public void UpdateState(ArrowState state)
    {
        if (curAction != PlayerFarmAction.NoActionNeeded)
        {
            dic[curAction].UpdateState(state);
        }
    }

    public void Close()
    {
        if (curAction != PlayerFarmAction.NoActionNeeded)
        {
            dic[curAction].Exit();
        }
    }

    public void Present(ActionLevel level)
    {
        var newTemp = Instantiate(Prefabs[level], Parent);
    }
}
