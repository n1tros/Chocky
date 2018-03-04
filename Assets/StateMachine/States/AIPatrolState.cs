using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class AIPatrolState : State
    {
        private AIController _ai;
        private State _nextState;
        private float _transitionTime;
        private IEnumerator _transitionCoroutine;

        private bool _movingRight = false;
        private float _rightDirection = 1;
        private float _leftDirection = -1;

        public AIPatrolState(AgentController agentcontoller) : base(agentcontoller)
        {
            _ai = _agentController.GetComponent<AIController>();
            _transitionTime = _ai.CurrentBrain.PatrolTransitionTime;
        }

        public override void OnEnter()
        {
            
        }

        public override void Tick()
        {
            if (_transitionCoroutine == null)
            {
                _transitionCoroutine = Transitiontimer();
                _ai.StartCoroutine(_transitionCoroutine);
            }
        }

        public override void FixedTick()
        {
            PatrolBetweenTwoPoints();
        }

        private IEnumerator Transitiontimer()
        {
            yield return new WaitForSeconds(_transitionTime);
            _ai.CurrentBrain.DefaultPatrolTransition(_ai);
        }

        private void PatrolBetweenTwoPoints()
        {
            if (_movingRight && AtRightEdge())
                _movingRight = false;

            if (!_movingRight && AtLeftEdge())
                _movingRight = true;

            if (_movingRight)
                _agentController.Move(_rightDirection);
            else
                _agentController.Move(_leftDirection);
        }

        private bool AtLeftEdge()
        {
            return _agentController.transform.position.x < _ai.LeftEdge.position.x;
        }

        private bool AtRightEdge()
        {
            return _agentController.transform.position.x > _ai.RightEdge.position.x;
        }

        public override void OnExit()
        {
            if (_transitionCoroutine != null)
                _ai.StopCoroutine(_transitionCoroutine);
        }
    }
}
