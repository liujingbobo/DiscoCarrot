using System;
using System.Collections;
using System.Collections.Generic;
using TimPlugin;
using UnityEngine;

public class ModularFarmTile : MonoBehaviour, IInteractable
{
    public InteractionTrigger interactionTrigger;
    private BasicStateMachine<string, ModularFarmTile> _farmTileStateMachine = new BasicStateMachine<string, ModularFarmTile>();
    
    public CropsDataObject[] CropsDatas;
    public CropsDataObject targetCropsData;
    public GameObject PlowedGameObject;
    public Transform farmTileCenter;
    
    [Header("ExposedValues")] public List<string> growthSequence = new List<string>();
    private void Start()
    {
        interactionTrigger.RegisterAsInteractable(this);
        _farmTileStateMachine.context = this;
        _farmTileStateMachine.AddStateInstance("Flattened", new FlattenedState());
        _farmTileStateMachine.AddStateInstance("Plowed", new PlowedState());
        _farmTileStateMachine.StartAtState("Flattened");
    }

    private void Update()
    {
        _farmTileStateMachine.ExecuteCurrentStateUpdate();
    }

    private void PlantSeed()
    {
        
    }
    
    public class FlattenedState : BasicStateInstance<string, ModularFarmTile>
    {
        public override void EnterState()
        {
            base.EnterState();
            Context.PlowedGameObject.SetActive(false);
        }

        public override void StateUpdate()
        {
            base.StateUpdate();
            if (Context.FPressed.Value)
            {
                SwitchToState("Plowed");
            }
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
    
    public class PlowedState : BasicStateInstance<string, ModularFarmTile>
    {
        public override void EnterState()
        {
            base.EnterState();
            Context.PlowedGameObject.SetActive(true);
        }
        public override void StateUpdate()
        {
            base.StateUpdate();
            if (Context.SeedPicked.Value)
            {
                Context.growthSequence.Clear();
                //planting new plant, add state to state machine
                foreach (var growthNode in Context.targetCropsData.growthNodes)
                {
                    var growthState = new CropGrowState();
                    growthState.growthNode = growthNode;
                    Context._farmTileStateMachine.AddStateInstance(growthNode.stateName, growthState);
                    Context.growthSequence.Add(growthNode.stateName);
                }
                SwitchToState(Context.growthSequence[0]);
            }
        }
        public override void ExitState()
        {
            base.ExitState();
        }
    }
    public class CropGrowState : BasicStateInstance<string, ModularFarmTile>
    {
        public CropsGrowthNode growthNode;
        private GameObject _presentationGO;
        public override void EnterState()
        {
            base.EnterState();
            _presentationGO = Instantiate(growthNode.presentationPrefab, Context.farmTileCenter);
        }
        public override void StateUpdate()
        {
            base.StateUpdate();
            if (Context.FPressed.Value)
            {
                Context.growthSequence.RemoveAt(0);
                if(Context.growthSequence.Count <= 0) SwitchToState("Flattened");
                else SwitchToState(Context.growthSequence[0]);
            }
        }
        public override void ExitState()
        {
            base.ExitState();
            if(_presentationGO) Destroy(_presentationGO);
            stateMachine.RemoveStateInstance(state);
        }
    }

    private BooleanUtility.OneUseFlag FPressed = new BooleanUtility.OneUseFlag();
    private BooleanUtility.OneUseFlag SeedPicked = new BooleanUtility.OneUseFlag();

    public void OnInteract()
    {
        Debug.Log($"timtest FPressed.Value");
        FPressed.Value = true;
    }
    public void OnAddMarkedInteractionObject(InteractionTrigger interactionTrigger) {}
    public void OnRemoveMarkedInteractionObject(InteractionTrigger interactionTrigger) {}

    public void PlantSeed(int i)
    {
        Debug.Log($"timtest SeedPicked.Value");
        targetCropsData = CropsDatas[i];
        SeedPicked.Value = true;
    }
}
