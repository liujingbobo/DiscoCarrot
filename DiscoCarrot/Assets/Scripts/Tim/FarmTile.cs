using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

    public GameObject GameEffect;
    public FarmTilePopUp popUp;
    //config values, set in editor
    public Vector2Int seedStateNeedWaterSpawnWindow; //define watering windows for seed events
    public Vector2Int[] sproutStateNeedsSpawnWindow; //define three windows for sprout events
    public int eventResponseTime;
    public int maxActionTime;

    public GameObject plowedSpriteGO;
    public GameObject seededSpriteGO;
    public GameObject sproutedSpriteGO;
    public List<GameObject> fullyGrownSpriteGOs; // index represent Carrot Grade

    public Transform leftTeleportPoint;
    public Transform rightTeleportPoint;
    public Transform upTeleportPoint;
    
    //exposed values, dont change it
    [Header("Exposed values, dont change it")]
    public FarmTilePlantState currentPlantState;
    public FarmTileEventFlag currentEventFlag;
    
    public int beatCountSinceSeeded = 0;
    public int turnToSproutedOnBeat; //relative to beatCountSinceSeeded
    public int turnToFullyGrownOnBeat; //relative to beatCountSinceSeeded
    public Dictionary<int, FarmTileEventFlag> BeatsToTriggerEvent = new Dictionary<int, FarmTileEventFlag>();

    public CarrotLevel fullyGrownCarrotLevel = CarrotLevel.Bad;
    public int totalPlantScore = 0;
    
    private void Start()
    {
        ResetFarmTile();
        GameEvents.OnOneBeatPassed += OnOneBeatPassed;
        GameEvents.OnFarmActionDone += OnFarmActionDone;
    }

    private void OnFarmActionDone(FarmTile targetTile, PlayerFarmAction action, ActionLevel level)
    {
        if (targetTile != this) return;
        switch (action)
        {
            case PlayerFarmAction.PlowLand:
                PlayEffectForDuration(1);
                SwitchPlantState(FarmTilePlantState.Plowed);
                break;
            case PlayerFarmAction.PlantSeed:
                PlayEffectForDuration(1);
                SwitchPlantState(FarmTilePlantState.Seeded);
                break;
            case PlayerFarmAction.WaterPlant:
                PlayEffectForDuration(1);
                SetFarmTileEvent(FarmTileEventFlag.NoEvent);
                break;
            case PlayerFarmAction.FertilizePlant:
                PlayEffectForDuration(1);
                SetFarmTileEvent(FarmTileEventFlag.NoEvent);
                break;
            case PlayerFarmAction.DebugPlant:
                PlayEffectForDuration(1);
                SetFarmTileEvent(FarmTileEventFlag.NoEvent);
                break;
            case PlayerFarmAction.HarvestPlant:
                //signal harvesting a carrot 
                PlayEffectForDuration(1);
                if(GameEvents.OnHarvestCarrot != null) GameEvents.OnHarvestCarrot.Invoke(fullyGrownCarrotLevel);
                ResetFarmTile();
                break;
        }
        
        //add score
        totalPlantScore += Config.GetScoreByActionLevel(level);

    }

    public Tween effectTween = null;
    public void PlayEffectForDuration(float second)
    {
        GameEffect.gameObject.SetActive(false);
        if(effectTween != null) effectTween.Kill();
        effectTween = DOTween.Sequence()
            .AppendCallback(() => { GameEffect.gameObject.SetActive(true); })
            .AppendInterval(second)
            .AppendCallback(() => { GameEffect.gameObject.SetActive(false); });
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
                fullyGrownCarrotLevel = Config.GetCarrotLevelByTotalScore(totalPlantScore);
                currentPlantState = FarmTilePlantState.FullyGrown;
                break;
        }
        UpdatePlantSpriteGO(state);
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
                //close pop up
                popUp.ClosePopUp();
                break;
            case FarmTileEventFlag.NeedWater:
            case FarmTileEventFlag.NeedFertilize:
            case FarmTileEventFlag.NeedDebug:
                //TODO: trigger pop up here
                popUp.StartPopUp(targetFlag, eventResponseTime);
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
        GameEffect.SetActive(false);
        beatCountSinceSeeded = 0;
        totalPlantScore = 0;
        BeatsToTriggerEvent.Clear();
        popUp.Reset();
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

        /*foreach (var pair in BeatsToTriggerEvent)
        {
            Debug.Log($"timtest beat:{pair.Key} , event: {pair.Value}");
        }*/
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

    public Transform GetTeleportPointTransform(PlayerFarmAction farmActionName)
    {
        switch (farmActionName)
        {
            case PlayerFarmAction.PlowLand: return leftTeleportPoint;
            case PlayerFarmAction.PlantSeed: return upTeleportPoint;
            case PlayerFarmAction.WaterPlant: return upTeleportPoint;
            case PlayerFarmAction.FertilizePlant: return leftTeleportPoint;
            case PlayerFarmAction.DebugPlant: return rightTeleportPoint;
            case PlayerFarmAction.HarvestPlant: return upTeleportPoint;
            default:
                return null;
        }
    }
}