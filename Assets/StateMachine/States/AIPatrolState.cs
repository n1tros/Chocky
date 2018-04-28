using System.Collections;
using UnityEngine;

namespace FSM
{
    public class AIPatrolState : AIState
    {
        private bool _movingRight = false;
        private float _rightDirection = 1;
        private float _leftDirection = -1;

        public AIPatrolState(AgentController agentcontoller) : base(agentcontoller)
        {
        }

        public override void OnEnter()
        {
            _ai.StartCoroutine(Transitiontimer());
        }

        public override void Tick()
        {
            base.Tick();
            PatrolBetweenTwoPoints();
        }

        private IEnumerator Transitiontimer()
        {
            yield return new WaitForSeconds(_ai.CurrentBrain.PatrolTransitionTime);
            _ai.CurrentBrain.DefaultPatrolTransition(_ai);
        }

        private void PatrolBetweenTwoPoints()
        {
            if (_movingRight && AtRightEdge())
                _movingRight = false;
            if (!_movingRight && AtLeftEdge())
                _movingRight = true;

            if (_movingRight)
                _agentController.MoveInput = _rightDirection;
            else
                _agentController.MoveInput = _leftDirection;
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
            _ai.StopCoroutine(Transitiontimer());
        }
    }
}
