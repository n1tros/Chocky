using System;
using UnityEngine;

namespace FSM
{
    public class StateFactory
    {
        public State GetAIState(AIStateType state)
        {
            return NewState(state);
        }

        private static State NewState(AIStateType state)
        {
            var stateType = Type.GetType(typeof(State).Namespace + "." + state.ToString() + "State", throwOnError: false);

            if (stateType == null)
                throw new InvalidOperationException(state.ToString() + " is not a valid state");

            if (!typeof(State).IsAssignableFrom(stateType))
                throw new InvalidOperationException(stateType.Name + " does not inherit from State");

            return (State)Activator.CreateInstance(stateType);
        }
    }
}

