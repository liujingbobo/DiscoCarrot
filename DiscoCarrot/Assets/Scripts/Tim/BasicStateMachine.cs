using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimPlugin
{
    public class BasicStateMachine<T_StateKey, TU_Context>
    {
        private Dictionary<T_StateKey, BasicStateInstance<T_StateKey,TU_Context>> stateDictionary = new Dictionary<T_StateKey, BasicStateInstance<T_StateKey,TU_Context>>();
        private T_StateKey _currentState;
        private T_StateKey _delayedNextState;
        
        public TU_Context context;
        
        public T_StateKey CurrentState
        {
            get => _currentState;
        }
        public BasicStateInstance<T_StateKey,TU_Context> AddStateInstance(T_StateKey state, BasicStateInstance<T_StateKey,TU_Context> basicStateInstance, bool showDebug = false)
        {
            basicStateInstance.stateMachine = this;
            basicStateInstance.state = state;
            stateDictionary[state] = basicStateInstance;
            basicStateInstance.isShowDebugLog = showDebug;
            return basicStateInstance;
        }

        public bool RemoveStateInstance(T_StateKey state)
        {
            if (stateDictionary.ContainsKey(state))
            {
                stateDictionary.Remove(state);
                return true;
            }
            return false;
        }
        
        public BasicStateInstance<T_StateKey,TU_Context> GetStateInstance(T_StateKey state)
        {
            if(stateDictionary.ContainsKey(state)) return stateDictionary[state];
            return null;
        }

        public void StartAtState(T_StateKey targetState)
        {
            _currentState = targetState;
            if(stateDictionary.ContainsKey(_currentState)) stateDictionary[_currentState].EnterState();
        }
        public void SwitchToState(T_StateKey targetState)
        {
            if(stateDictionary.ContainsKey(_currentState)) stateDictionary[_currentState].ExitState();
            _currentState = targetState;
            if(stateDictionary.ContainsKey(_currentState)) stateDictionary[_currentState].EnterState();
        }
        
        public void ExecuteCurrentStateUpdate()
        {
            if(stateDictionary.ContainsKey(_currentState)) stateDictionary[_currentState].StateUpdate();
        }
    }
    
        
    public class BasicStateInstance<T_StateKey, TU_Context>
    {
        public BasicStateMachine<T_StateKey, TU_Context> stateMachine;
        public T_StateKey state;
        public bool isShowDebugLog = false;
        protected TU_Context Context
        {
            get => stateMachine.context;
        }
        public virtual void EnterState()
        {
            if(isShowDebugLog) Debug.Log($"State [{state}] EnterState Executing");
        }

        public virtual void ExitState()
        {
            if(isShowDebugLog) Debug.Log($"State [{state}] ExitState Executing");

        }

        public virtual void StateUpdate()
        {
            if(isShowDebugLog) Debug.Log($"State [{state}] StateUpdate Executing");

        }

        public virtual void StateFixedUpdate()
        {
            if(isShowDebugLog) Debug.Log($"State {state} StateFixedUpdate Executing");

        }
            
        public virtual void SwitchToState(T_StateKey targetState)
        {
            if(isShowDebugLog) Debug.Log($"State [{state}] SwitchToState [{targetState}] Executing");
            stateMachine.SwitchToState(targetState);
        }
    }
}

