using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class AIMeleeAttackState : State
    {
        AgentController _target = null;
        AIController _ai = null;

        public AIMeleeAttackState(AgentController agentcontoller) : base(agentcontoller)
        {
        }

        public override void OnEnter()
        {
            _ai = _agentController.GetComponent<AIController>();
            _target = _ai.Target;
            _agentController.DrawWeapon(true);
        }

        public override void FixedTick()
        {
            if (_ai.TargetInMeleeRange)
            {
                _agentController.Idle();
                _agentController.Attack();
            }
            else
                _agentController.Move(_agentController.transform.position.x < _target.transform.position.x ? 1 : -1);
        }
    }
}

