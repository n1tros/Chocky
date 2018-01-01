using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class AIStartState : State
    {
        public override void OnEnter(AgentController agent)
        {
            Debug.Log(" Entering AIStartState");
        }

        public override void OnExit()
        {
        }

        public override void Update()
        {
        }

        public override void FixedUpdate()
        {
        }

    }
}

