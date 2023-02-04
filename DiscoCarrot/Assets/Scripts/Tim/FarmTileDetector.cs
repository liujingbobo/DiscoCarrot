using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmTileDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("FarmTile"))
        {
            var farmTile = other.GetComponent<FarmTile>();
            var neededAction = farmTile.GetNeededPlayerFarmAction();
            if(GameEvents.OnReachedFarmTile != null) GameEvents.OnReachedFarmTile.Invoke(farmTile,neededAction);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("FarmTile"))
        {
            var farmTile = other.GetComponent<FarmTile>();
            if(GameEvents.OnLeaveFarmTile != null) GameEvents.OnLeaveFarmTile.Invoke(farmTile);
        }
    }
}
