using UnityEngine;

namespace FSM
{
    public class StateMachine
    {
        private State _currentState;
        public State CurrentState { get { return _currentState; } }

        private State _previousState;
        public State PreviousState { get { return _previousState; } }

        private AgentController _agent = null;

        public StateMachine(AgentController agent)
        {
            _agent = agent;
        }

        public void ChangeState(State newState)
        {
            if (_currentState == null)
                ChangeCurrentStateAndCallOnEnter(newState);

            else if (_currentState.GetType() != newState.GetType())
            {
                ExitPreviousState();
                Debug.Log(_agent.transform.parent.name + " changing state to " + newState.GetType().Name);
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