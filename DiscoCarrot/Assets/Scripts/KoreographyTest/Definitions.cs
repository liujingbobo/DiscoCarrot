using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
    Normal,
    Hard,
    Hell
}

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
    public static CarrotLevel GetCarrotLevelByTotalScore(int totalScore)
    {
        var scoreRatio = totalScore / (float) MAX_PLANT_SCORE;
        if (scoreRatio >= 1) return CarrotLevel.Disco;
        if (scoreRatio >= 0.9f) return CarrotLevel.Muscle;
        if (scoreRatio >= 0.8f) return CarrotLevel.Normal;
        return CarrotLevel.Bad;
    }
    
    public static int GetScoreByCarrotLevel(CarrotLevel level)
    {
        switch (level)
        {
            case CarrotLevel.Disco:return 100;
            case CarrotLevel.Muscle:return 80;
            case CarrotLevel.Normal:return 50;
            case CarrotLevel.Bad:return 30;
        }
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
    void Pass(ArrowState state);
    void Exit();
    void Reset();
}
