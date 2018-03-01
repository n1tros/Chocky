using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class AIDeathState : State
    {
        AgentController _agent = null;
        BoxCollider2D[] _colliders = null;

        public override void OnEnter(AgentController agent)
        {
            _agent = agent;
            _agent.DrawWeapon(false);
            _agent.gameObject.layer = 17;
            _agent.GetComponent<Rigidbody2D>().isKinematic = true;
            

            //TODO: Might need to target specific colliders
            /*
            _colliders = _agent.GetComponentsInChildren<BoxCollider2D>();

            foreach (var col in _colliders)
            {
                col.enabled = false;
            }
            */
        }

        public override void FixedUpdate()
        {

        }

        public override void Tick()
        {
        }

        public override void OnExit()
        {
            /*
            foreach (var col in _colliders)
            {
                col.enabled = true;
            }
            */
        }
    }
}


