using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FarmTile : MonoBehaviour
{
    public enum FarmTilePlantState
    {
        Empty,
        Plowed,
        Seeded,
        Sprouted,
        FullyGrown,
    }
    public enum FarmTileEventFlag
    {
        NoEvent,
        NeedWater,
        NeedFertilize,
        NeedDebug,
    }
    
    //config values, set in editor
    public Vector2Int seedStateNeedWaterSpawnWindow; //define watering windows for seed events
    public Vector2Int[] sproutStateNeedsSpawnWindow; //define three windows for sprout events
    public int eventResponseTime;
    public int maxActionTime;

    public GameObject plowedSpriteGO;
    public GameObject seededSpriteGO;
    public GameObject sproutedSpriteGO;
    public List<GameObject> fullyGrownSpriteGOs; // index represent Carrot Grade
    public List<GameObject> eventFlagSpriteGOs; // index + 1 represent FarmTileEventFlag

    //exposed values, dont change it
    [Header("Exposed values, dont change it")]
    public FarmTilePlantState currentPlantState;
    public FarmTileEventFlag currentEventFlag;
    
    public int beatCountSinceSeeded = 0;
    public int turnToSproutedOnBeat; //relative to beatCountSinceSeeded
    public int turnToFullyGrownOnBeat; //relative to beatCountSinceSeeded
    public Dictionary<int, FarmTileEventFlag> BeatsToTriggerEvent = new Dictionary<int, FarmTileEventFlag>();

    public CarrotLevel fullyGrownCarrotLevel = CarrotLevel.Bad;
    
    private void Start()
    {
        ResetFarmTile();
        GameEvents.OnOneBeatPassed += OnOneBeatPassed;
        GameEvents.OnFarmActionDone += OnFarmActionDone;
    }

    private void OnFarmActionDone(FarmTile targetTile, PlayerFarmAction action, ActionLevel score)
    {
        if (targetTile != this) return;
        switch (action)
        {
            case PlayerFarmAction.PlowLand:
                SwitchPlantState(FarmTilePlantState.Plowed);
                break;
            case PlayerFarmAction.PlantSeed:
                SwitchPlantState(FarmTilePlantState.Seeded);
                break;
            case PlayerFarmAction.WaterPlant:
                SetFarmTileEvent(FarmTileEventFlag.NoEvent);
                break;
            case PlayerFarmAction.FertilizePlant:
                SetFarmTileEvent(FarmTileEventFlag.NoEvent);
                break;
            case PlayerFarmAction.DebugPlant:
                SetFarmTileEvent(FarmTileEventFlag.NoEvent);
                break;
            case PlayerFarmAction.HarvestPlant:
                ResetFarmTile();
                break;
        }
        
    }

    private void SwitchPlantState(FarmTilePlantState state)
    {
        switch (state)
        {
                case FarmTilePlantState.Empty:
                    currentPlantState = FarmTilePlantState.Empty;
                    break;
                case FarmTilePlantState.Plowed:
                    currentPlantState = FarmTilePlantState.Plowed;
                    break;
                case FarmTilePlantState.Seeded:
                    beatCountSinceSeeded = 0;
                    currentPlantState = FarmTilePlantState.Seeded;
                    break;
                case FarmTilePlantState.Sprouted:
                    currentPlantState = FarmTilePlantState.Sprouted;
                    break;
                case FarmTilePlantState.FullyGrown:
                    //pick grown type
                    fullyGrownCarrotLevel = CalculateFinalCarrotLevel();
                    currentPlantState = FarmTilePlantState.FullyGrown;
                    break;
        }
        UpdatePlantSpriteGO(state);
    }
    
    public CarrotLevel CalculateFinalCarrotLevel()
    {
        return CarrotLevel.Bad;
    }
    
    private void UpdatePlantSpriteGO(FarmTilePlantState state)
    {
        switch (state)
        {
            case FarmTilePlantState.Empty:
                plowedSpriteGO.SetActive(false);
                seededSpriteGO.SetActive(false);
                sproutedSpriteGO.SetActive(false);
                foreach (var g in fullyGrownSpriteGOs)
                {
                    g.SetActive(false);
                }
                break;
            case FarmTilePlantState.Plowed:
                plowedSpriteGO.SetActive(true);
                break;
            case FarmTilePlantState.Seeded:
                seededSpriteGO.SetActive(true);
                break;
            case FarmTilePlantState.Sprouted:
                seededSpriteGO.SetActive(false);
                sproutedSpriteGO.SetActive(true);
                break;
            case FarmTilePlantState.FullyGrown:
                sproutedSpriteGO.SetActive(false);
                fullyGrownSpriteGOs[(int)fullyGrownCarrotLevel].SetActive(true);
                break;
        }
    }
    private void SetFarmTileEvent(FarmTileEventFlag targetFlag)
    {
        switch (targetFlag)
        {
            case FarmTileEventFlag.NoEvent:
                if (currentEventFlag == FarmTileEventFlag.NoEvent) return;
                //TODO close pop up
                //stop all sprite GO
                foreach (var g in eventFlagSpriteGOs)
                {
                    g.SetActive(false);
                }
                break;
            case FarmTileEventFlag.NeedWater:
            case FarmTileEventFlag.NeedFertilize:
            case FarmTileEventFlag.NeedDebug:
                //TODO: trigger pop up here
                
                //show sprite GO
                eventFlagSpriteGOs[(int) targetFlag - 1].SetActive(true);
                break;
        }
        currentEventFlag = targetFlag;
    }
    
    private void OnOneBeatPassed()
    {
        //add beat count since seeded
        if (currentPlantState != FarmTilePlantState.Empty && currentPlantState != FarmTilePlantState.Plowed)
        {
            beatCountSinceSeeded += 1;
        }
        
        //check plant state and beats to grow
        if (currentPlantState == FarmTilePlantState.Seeded && beatCountSinceSeeded == turnToSproutedOnBeat)
        {
            SwitchPlantState(FarmTilePlantState.Sprouted);
        }
        if (currentPlantState == FarmTilePlantState.Sprouted && beatCountSinceSeeded == turnToFullyGrownOnBeat)
        {
            SwitchPlantState(FarmTilePlantState.FullyGrown);
        }
        
        //check if this beat will trigger event
        if (BeatsToTriggerEvent.ContainsKey(beatCountSinceSeeded))
        {
            SetFarmTileEvent(BeatsToTriggerEvent[beatCountSinceSeeded]);
        }
    }
    
    public void ResetFarmTile()
    {
        SwitchPlantState(FarmTilePlantState.Empty);
        beatCountSinceSeeded = 0;
        
        BeatsToTriggerEvent.Clear();
        turnToSproutedOnBeat = 0;
        turnToFullyGrownOnBeat = 0;
        SetRandomBeatTriggerValues();
    }
    
    [ExecuteInEditMode]
    [ContextMenu("RunSetRandomBeatTriggerValues")]
    public void SetRandomBeatTriggerValues()
    {
        var lastestActionEndBeatLength = eventResponseTime + maxActionTime;
        turnToSproutedOnBeat = seedStateNeedWaterSpawnWindow.y + lastestActionEndBeatLength + 8;
        var seedWateringEventBeat = Random.Range(seedStateNeedWaterSpawnWindow.x, seedStateNeedWaterSpawnWindow.y);
        BeatsToTriggerEvent[seedWateringEventBeat] = FarmTileEventFlag.NeedWater;
        BeatsToTriggerEvent[seedWateringEventBeat+eventResponseTime] = FarmTileEventFlag.NoEvent;
        
        var tmpEndBeat = turnToSproutedOnBeat;
        List<FarmTileEventFlag> eventOptions = new List<FarmTileEventFlag>()
            {FarmTileEventFlag.NeedWater, FarmTileEventFlag.NeedFertilize, FarmTileEventFlag.NeedDebug};
        foreach (var window in sproutStateNeedsSpawnWindow)
        {
            Vector2Int actualWindow = new Vector2Int(window.x + tmpEndBeat, window.y + tmpEndBeat);
            
            var triggerEventBeat = Random.Range(actualWindow.x, actualWindow.y);
            var randomEventIndex = Random.Range(0,eventOptions.Count);
            BeatsToTriggerEvent[triggerEventBeat] = eventOptions[randomEventIndex];
            BeatsToTriggerEvent[triggerEventBeat+eventResponseTime] = FarmTileEventFlag.NoEvent;
            eventOptions.RemoveAt(randomEventIndex);
            
            tmpEndBeat += window.y + lastestActionEndBeatLength + 8;
        }

        turnToFullyGrownOnBeat = tmpEndBeat;

        foreach (var pair in BeatsToTriggerEvent)
        {
            Debug.Log($"timtest beat:{pair.Key} , event: {pair.Value}");
        }
    }
    
    //public apis
    public PlayerFarmAction GetNeededPlayerFarmAction()
    {
        if (currentEventFlag != FarmTileEventFlag.NoEvent)
        {
            switch (currentEventFlag)
            {
                case FarmTileEventFlag.NeedWater: return PlayerFarmAction.WaterPlant;
                case FarmTileEventFlag.NeedDebug: return PlayerFarmAction.DebugPlant;
                case FarmTileEventFlag.NeedFertilize: return PlayerFarmAction.FertilizePlant;
            }
        }
        switch (currentPlantState)
        {
            case FarmTilePlantState.Empty: return PlayerFarmAction.PlowLand;
            case FarmTilePlantState.Plowed: return PlayerFarmAction.PlantSeed;
            case FarmTilePlantState.Seeded: return PlayerFarmAction.NoActionNeeded;
            case FarmTilePlantState.Sprouted: return PlayerFarmAction.NoActionNeeded;
            case FarmTilePlantState.FullyGrown: return PlayerFarmAction.HarvestPlant;
        }
        return PlayerFarmAction.NoActionNeeded;
    }
}