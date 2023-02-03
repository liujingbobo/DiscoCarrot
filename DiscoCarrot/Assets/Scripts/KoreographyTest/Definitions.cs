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

public enum ArrowLevel
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
    Harvest,
}

public interface IState
{
    void Enter();
    void Exit();
    void Reset();
}
