using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public Vector2Int seedStateNeedWaterSpawnWindow; //define watering windows for seed events
    public Vector2Int[] sproutStateNeedsSpawnWindow; //define three windows for sprout events
    public int eventResponseTime;
    public int maxActionTime;

    public GameObject plowedSpriteGO;
    public GameObject seededSpriteGO;
    public GameObject sproutedSpriteGO;
    public GameObject needWaterSpriteGO;
    public GameObject needFertilizeSpriteGO;
    public GameObject needDebugSpriteGO;
    public GameObject fullyGrownSpriteGO;
    
    
    //exposed values, dont change it
    [Header("Exposed values, dont change it")]
    public FarmTileState currentFarmTileState;
    public int beatCountSinceSeeded = 0;
    public int beatCountSinceLastEvent = 0;

    //beat that trigger certian effect, set when reseting farmtile
    public Dictionary<int, FarmTileState> beatToTriggerNeeds = new Dictionary<int, FarmTileState>();
    public int turnToSproutedBeat;
    public int turnToFullyGrownBeat;
    
    private void Start()
    {
        ResetFarmTile();
        GameEvents.OnOneBeatPassed += OnOneBeatPassed;
    }

    private void OnOneBeatPassed()
    {
        if (currentFarmTileState != FarmTileState.Empty || currentFarmTileState != FarmTileState.Plowed)
        {
            beatCountSinceSeeded += 1;
        }
        //check if on beats that need to change state
        if (beatCountSinceSeeded == turnToSproutedBeat && currentFarmTileState == FarmTileState.Seeded)
        {
            currentFarmTileState = FarmTileState.Sprouted;
        }
        if (beatCountSinceSeeded == turnToFullyGrownBeat && currentFarmTileState == FarmTileState.Sprouted)
        {
            currentFarmTileState = FarmTileState.FullyGrown;
        }

        UpdatePlantGO();
    }

    private void UpdatePlantGO()
    {
        if (currentFarmTileState == FarmTileState.Seeded)
        {
            seededSpriteGO.SetActive(true);
        }
        if (currentFarmTileState == FarmTileState.Sprouted)
        {
            seededSpriteGO.SetActive(false);
            sproutedSpriteGO.SetActive(true);
        }
        if (currentFarmTileState == FarmTileState.FullyGrown)
        {
            sproutedSpriteGO.SetActive(false);
            fullyGrownSpriteGO.SetActive(true);
        }
    }
    
    public void ResetFarmTile()
    {
        currentFarmTileState = FarmTileState.Empty;
        beatCountSinceSeeded = 0;
        
        beatToTriggerNeeds.Clear();
        turnToSproutedBeat = 0;
        turnToFullyGrownBeat = 0;
        SetRandomBeatTriggerValues();
    }
    
    [ExecuteInEditMode]
    [ContextMenu("RunSetRandomBeatTriggerValues")]
    public void SetRandomBeatTriggerValues()
    {
        var lastestActionEndBeatLength = eventResponseTime + maxActionTime;
        turnToSproutedBeat = seedStateNeedWaterSpawnWindow.y + lastestActionEndBeatLength;
        var seedWateringEventBeat = Random.Range(seedStateNeedWaterSpawnWindow.x, seedStateNeedWaterSpawnWindow.y);
        beatToTriggerNeeds[seedWateringEventBeat] = FarmTileState.NeedWater;
        
        var tmpEndBeat = turnToSproutedBeat;
        List<FarmTileState> stateOptions = new List<FarmTileState>()
            {FarmTileState.NeedWater, FarmTileState.NeedFertilize, FarmTileState.NeedDebug};
        
        foreach (var window in sproutStateNeedsSpawnWindow)
        {
            tmpEndBeat = tmpEndBeat + window.y + lastestActionEndBeatLength;
            var triggerEventBeat = Random.Range(window.x, window.y);
            var randomEventIndex = Random.Range(0,stateOptions.Count);
            beatToTriggerNeeds[triggerEventBeat] = stateOptions[randomEventIndex];
            stateOptions.RemoveAt(randomEventIndex);
        }

        turnToFullyGrownBeat = tmpEndBeat;
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