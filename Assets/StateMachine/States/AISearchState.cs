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
        IEnumerator _turningCoroutine, _transitionCoroutine;
        AIController _ai;
        private float _transitionTime;

        public AISearchState(AgentController agentcontoller) : base(agentcontoller)
        {
            _ai = _agentController.GetComponent<AIController>();
            _transitionTime = _ai.CurrentBrain.SearchTransitionTime;
        }

        public override void OnEnter()
        {
            Debug.Log(_agentController.transform.parent.name + " Entering Search state");
        }

        public override void Tick()
        {
            if (_transitionCoroutine == null)
            {
                _transitionCoroutine = Transition();
                _ai.StartCoroutine(_transitionCoroutine);
            }

        }

        private IEnumerator Transition()
        {
            yield return new WaitForSeconds(_transitionTime);
            _ai.CurrentBrain.DefaultSearchTransition(_ai);
        }

        public override void FixedTick()
        {
            if(_turningCoroutine == null)
            {
                _turningCoroutine = TurnDelay();
                _ai.StartCoroutine(_turningCoroutine);
            }
        }

        IEnumerator TurnDelay()
        {
            _agentController.Idle();
            yield return new WaitForSeconds(2f); //TODO: Get delay from brain settings
            _agentController.transform.localScale = new Vector3(_agentController.transform.localScale.x * -1, 1, 1);
        }

        public override void OnExit()
        {
            if (_turningCoroutine != null)
                _ai.StopCoroutine(_turningCoroutine);
            if (_transitionCoroutine != null)
                _ai.StopCoroutine(_transitionCoroutine);
        }
    }
}

