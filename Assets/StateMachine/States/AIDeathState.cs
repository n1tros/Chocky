using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class AIDeathState : State
    {
        int _deathLayer = 17;

        public AIDeathState(AgentController agentcontoller) : base(agentcontoller)
        {
        }

        public override void OnEnter()
        {
            _agentController.DrawWeapon(false);
            _agentController.gameObject.layer = _deathLayer;
            _agentController.GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }
}


