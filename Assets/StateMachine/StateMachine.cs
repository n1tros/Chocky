using UnityEngine;

namespace FSM
{
    public class StateMachine
    {
        private State _currentState;
        public State CurrentState { get { return _currentState; } }

        private State _previousState;
        public State PreviousState { get { return _previousState; } }

        public void ChangeState(State newState)
        {
            if (_currentState == null)
                ChangeCurrentStateAndCallOnEnter(newState);

            else if (_currentState.GetType() != newState.GetType())
            {
                ExitPreviousState();
                //Debug.Log(this.GetType() + " Enter newState " + newState.GetType());

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