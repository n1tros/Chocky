using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class StateMachine
    {
        private State _currentState;
        public State CurrentState { get { return _currentState; } }

        private State _previousState;
        public State PreviousState { get { return _previousState; } }

        private StateFactory _stateFactory = null;
        private AgentController _agent = null;

        public StateMachine(AgentController agent)
        {
            _stateFactory = new StateFactory();
            _agent = agent;
        }

        public void ChangeState(State newState)
        {
            if (_currentState == null)
                ChangeCurrentStateAndCallOnEnter(newState);

            else if (_currentState != newState)
            {
                ExitPreviousState();
                ChangeCurrentStateAndCallOnEnter(newState);
            }
        }

        private void ExitPreviousState()
        {
            _currentState.OnExit();
            _previousState = _currentState;
        }

        private void ChangeCurrentStateAndCallOnEnter(State newState)
        {
            _currentState = newState;
            _currentState.OnEnter();
        }
    }
}