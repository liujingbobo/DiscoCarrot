using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimPlugin
{
    public class SimpleStateMachine<T_StateKey, TU_Context>
    {
        private Dictionary<T_StateKey, SimpleStateInstance<T_StateKey,TU_Context>> stateDictionary = new Dictionary<T_StateKey, SimpleStateInstance<T_StateKey,TU_Context>>();
        private T_StateKey _currentState;
        public TU_Context sharedContext;
        
        public SimpleStateInstance<T_StateKey,TU_Context> AddStateInstance(T_StateKey state, SimpleStateInstance<T_StateKey,TU_Context> simpleStateInstance, bool showDebug = false)
        {
            simpleStateInstance.stateMachine = this;
            simpleStateInstance.state = state;
            stateDictionary[state] = simpleStateInstance;
            simpleStateInstance.isShowDebugLog = showDebug;
            return simpleStateInstance;
        }

        public SimpleStateInstance<T_StateKey,TU_Context> GetStateInstance(T_StateKey state)
        {
            if(stateDictionary.ContainsKey(state)) return stateDictionary[state];
            return null;
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
    
        
    public class SimpleStateInstance<T_StateKey, TU_Context>
    {
        public SimpleStateMachine<T_StateKey, TU_Context> stateMachine;
        public T_StateKey state;
        public bool isShowDebugLog = false;

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

