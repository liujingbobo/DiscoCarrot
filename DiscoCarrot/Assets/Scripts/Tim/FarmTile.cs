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
    
    
    //public apis
    public PlayerFarmAction GetNeededPlayerFarmAction()
    {
        return PlayerFarmAction.PlowLand;
    }
    
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
}


public enum PlayerFarmAction
{
    PlowLand,
    PlantSeed,
    WaterPlant,
    FertilizePlant,
    DebugPlant,
    HarvestPlant,
}