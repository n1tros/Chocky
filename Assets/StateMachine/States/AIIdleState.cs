using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class AIIdleState : State
    {
        Rigidbody2D _rigid;
        AIController _ai;
        State _nextState;
        float _transitionTime;
        IEnumerator _transitionCoroutine;

        public AIIdleState(AgentController agentcontoller) : base(agentcontoller)
        {
            _rigid = _agentController.GetComponent<Rigidbody2D>();
            _ai = _agentController.GetComponent<AIController>();
            _transitionTime = _ai.CurrentBrain.IdleTransitionTime;
        }

        public override void OnEnter()
        {
            Debug.Log("Entering Idle State");
            _agentController.Idle();
        }

        public override void Tick()
        {
            if (_transitionCoroutine == null)
            {
                _transitionCoroutine = TransitionToDefaultState();
                _ai.StartCoroutine(_transitionCoroutine);
            }
        }

        IEnumerator TransitionToDefaultState()
        {
            yield return new WaitForSeconds(_transitionTime);
            _ai.CurrentBrain.DefaultIdleTransition(_ai);
        }

        public override void FixedTick()
        {
            _rigid.velocity = Vector2.zero;
        }

        public override void OnExit()
        {
            if (_transitionCoroutine != null)
                _ai.StopCoroutine(_transitionCoroutine);
        }

    }
}

