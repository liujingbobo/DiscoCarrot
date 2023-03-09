using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum CarrotLevel
{
    Disco,
    Muscle,
    Normal,
    Bad
}

public enum PressLevel
{
    Perfect,
    Good,
    Miss
}

public enum ActionLevel
{
    Perfect,
    Good,
    Miss
}

public enum PlayerFarmAction
{
    NoActionNeeded,
    PlowLand,
    PlantSeed,
    WaterPlant,
    FertilizePlant,
    DebugPlant,
    HarvestPlant,
}

public static class Config
{
    public static Dictionary<PlayerFarmAction, PlayerAnimName> tmpFarmActionToAnim =
        new Dictionary<PlayerFarmAction, PlayerAnimName>()
        {
            {PlayerFarmAction.NoActionNeeded,PlayerAnimName.Idle},
            {PlayerFarmAction.PlowLand,PlayerAnimName.PlowDown},
            {PlayerFarmAction.PlantSeed,PlayerAnimName.PlantSeed },
            {PlayerFarmAction.WaterPlant,PlayerAnimName.Watering },
            {PlayerFarmAction.FertilizePlant,PlayerAnimName.Fertilize },
            {PlayerFarmAction.DebugPlant,PlayerAnimName.Debugging },
            {PlayerFarmAction.HarvestPlant,PlayerAnimName.Harvest1 },
        };

    public static int GetScoreByActionLevel(ActionLevel level)
    {
        switch (level)
        {
            case ActionLevel.Perfect:return 5;
            case ActionLevel.Good:return 3;
            case ActionLevel.Miss:return 0;
        }
        return 0;
    }
    
    public const int MAX_PLANT_SCORE = 35;
    public const int MISSED_DEDUCT_SCORE = -10;
    public static CarrotLevel GetCarrotLevelByTotalScore(int totalScore)
    {
        if (totalScore >= 33) return CarrotLevel.Disco;
        if (totalScore >= 28) return CarrotLevel.Muscle;
        if (totalScore >= 23) return CarrotLevel.Normal;
        return CarrotLevel.Bad;
    }
    
    public static int GetScoreByCarrotLevel(CarrotLevel level)
    {
        switch (level)
        {
            case CarrotLevel.Disco:return 700;
            case CarrotLevel.Muscle:return 400;
            case CarrotLevel.Normal:return 200;
            case CarrotLevel.Bad:return 100;
        }
        return 0;
    }
    
    public static int GetGradeFromTotalScore(int totalScore)
    {
        if (totalScore >= 5600) return 3;
        if (totalScore >= 2800) return 2;
        if (totalScore >= 1400) return 1;
        return 0;
    }
}
public enum PlayerAnimName
{
    Idle,
    Move,
    Sad, //miss灰心
    PlowUp,
    PlowDown,
    PlantSeed,
    Watering,
    Debugging,
    ReadyFertilize, //预备施肥
    Fertilize, //施肥
    Harvest0,
    Harvest1,
}
public interface IState
{
    void Enter();
    void Exit();
    void Reset();
    void UpdateState();
}
public interface IUIThumbnail<T>
{
    void FillWith(T target, params object[] extraInfos);
}
public enum ArrowState
{
    Normal,
    Miss,
    Good,
    Perfect
}
public interface IIndicator
{
    void Init();
    void UpdateState(ArrowState state);
    void Exit();
    void Reset();
}
public enum BeatType
{
    Down, // The first beat of the bar
    Up
}
public enum BeatActionType
{
    None,
    Click, 
    Hold
}
public enum ArrowDirection
{
    Up,
    Right,
    Down,
    Left
}
public struct BeatAction
{
    public BeatActionType Action;
    [HideIf("Action", BeatActionType.None)]public ArrowDirection Direction;
}
