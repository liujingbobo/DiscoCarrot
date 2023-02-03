using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmTile : MonoBehaviour
{
    public enum FarmTileState
    {
        Empty,
        Plowed,
        Seeded,
        Sprouted,
        NeedWater,
        NeedFertilize,
        NeedDebug,
        FullyGrown,
    }
    
    //config values, set in editor
    public int totalGrowthBeatLength;
    public int SeededToSproutedBeatLength;
    public int SproutedToFullyGrownBeatLength;

    public FarmTileState currentFarmTileState;


    //exposed values, dont change it
    public int beatCountSinceSeeded = 0;
    
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void ResetFarmTile()
    {
        currentFarmTileState = FarmTileState.Empty;
        beatCountSinceSeeded = 0;
        
    }
    
    //public apis
    public PlayerFarmAction GetNeededPlayerFarmAction()
    {
        switch (currentFarmTileState)
        {
            case FarmTileState.Empty: return PlayerFarmAction.PlowLand;
            case FarmTileState.Plowed: return PlayerFarmAction.PlantSeed;
            case FarmTileState.Seeded: return PlayerFarmAction.NoActionNeeded;
            case FarmTileState.Sprouted: return PlayerFarmAction.NoActionNeeded;
            case FarmTileState.NeedWater: return PlayerFarmAction.WaterPlant;
            case FarmTileState.NeedFertilize: return PlayerFarmAction.FertilizePlant;
            case FarmTileState.NeedDebug: return PlayerFarmAction.DebugPlant;
            case FarmTileState.FullyGrown: return PlayerFarmAction.HarvestPlant;
        }
        return PlayerFarmAction.PlowLand;
    }

    public CarrotLevel GetFinalCarrotLevel()
    {
        return CarrotLevel.Bad;
    }
}