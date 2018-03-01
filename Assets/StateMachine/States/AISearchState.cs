using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    /// <summary>
    /// Class that will search after loosing the target, it will delay before turning.
    /// </summary>
    public class AISearchState : State
    {
        AgentController _agent = null;
        bool _turning = false;
        
        public override void FixedUpdate()
        {
            if(!_turning)
                _agent.StartCoroutine(TurnDelay());
        }

        IEnumerator TurnDelay()
        {
            _turning = true;
            _agent.Idle();
            yield return new WaitForSeconds(2f); //TODO: Get delay from weapon type
            _agent.transform.localScale = new Vector3(_agent.transform.localScale.x * -1, 1, 1);
            _turning = false;
        }

        public override void OnEnter(AgentController agent)
        {
            _agent = agent;
            Debug.Log(_agent.transform.parent.name + " Entering Search state");
        }

        public override void OnExit()
        {
            //TODO: Target and stop this particular routine.
            _agent.StopAllCoroutines();
        }

        public override void Tick()
        {
        }
    }
}

