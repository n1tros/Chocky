using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class AIStateMachine
    {
        public AIStateType CurrentState { get; protected set; }
        public AIStateType PreviousState { get; protected set; }

        private State _currentState = null;
        private StateFactory _stateFactory = null;
        private AgentController _agent = null;

        public AIStateMachine(AgentController agent)
        {
            _stateFactory = new StateFactory();
            _agent = agent;
            _currentState = _stateFactory.GetAIState(AIStateType.AIStart);
        }

        public void ChangeState(AIStateType newState)
        {
            if (CurrentState != newState)
            {
                _currentState.OnExit();
                _currentState = _stateFactory.GetAIState(newState);
                PreviousState = CurrentState;
                CurrentState = newState;
                _currentState.OnEnter(_agent);
            }
        }

        public void Update()
        {
            _currentState.Update();
        }

        public void FixedUpdate()
        {
            _currentState.FixedUpdate();
        }
    }
}