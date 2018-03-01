using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class AIDamageState : State
    {

        Rigidbody2D _rigid = null;

        public override void OnEnter(AgentController agent)
        {
            Debug.Log("Entering AIDamageState");
            _rigid = agent.GetComponent<Rigidbody2D>();
            //agent.AgentIdle();
        }

        public override void OnExit()
        {
        }

        public override void Tick()
        {
        }

        public override void FixedUpdate()
        {
            _rigid.velocity = Vector2.zero;
        }

    }
}
