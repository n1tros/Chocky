using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class AIDamageState : State
    {
        Rigidbody2D _rigid = null;

        public AIDamageState(AgentController agentcontoller) : base(agentcontoller)
        {
        }

        public override void OnEnter()
        {
            _rigid = _agentController.GetComponent<Rigidbody2D>();
        }

        public override void FixedTick()
        {
            _rigid.velocity = Vector2.zero;
        }
    }
}
