using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static Action OnOneBeatPassed;
    public static Action OnOneTickPassed;
    public static Action OnDownBeat;
    public static Action OnUpBeat;

    public static Action<FarmTile, PlayerFarmAction> OnReachedFarmTile;
    public static Action<FarmTile> OnLeaveFarmTile;

    public static Action<FarmTile, PlayerFarmAction, ActionLevel> OnFarmActionDone;

    public static Action<CarrotLevel> OnHarvestCarrot;
    // ULTIMATE HARDCODE
    public static bool isClockWise; // for debug UI, don't worry abt it
}
