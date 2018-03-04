using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class AIStartState : State
    {
        private AIController _ai;

        public AIStartState(AgentController agentcontoller) : base(agentcontoller)
        {
        }

        public override void OnEnter()
        {
            Debug.Log(" Entering AIStartState " + _agentController.gameObject.name.ToString());
            _ai = _agentController.GetComponent<AIController>();
            _ai.StateMachine.ChangeState(new AIIdleState(_agentController));
        }
    }
}

