using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class AIRangedAttackState : State
    {
        AgentController _target, _agent = null;
        AIController _ai = null;

        public override void OnEnter(AgentController agent)
        {
            _ai = agent.GetComponent<AIController>();
            _target = _ai.Target;
            _agent = agent;
            _agent.DrawWeapon(true);
        }

        public override void FixedUpdate()
        {
            if (_ai.Target != null)
            {
                _agent.Idle();
                Debug.Log("Attacking");
                _ai.CurrentBrain.AttackPattern(_ai);
            }
            else
                _agent.Move(_agent.transform.position.x < _target.transform.position.x ? 1 : -1);
        }

        public override void Tick()
        {
        }

        public override void OnExit()
        {
        }
    }
}

