using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class AIPatrolState : State
    {
        private AgentController _agent;
        private AIController _ai;

        private bool _movingRight = false;
        private float _rightDirection = 1;
        private float _leftDirection = -1;

        public override void OnEnter(AgentController agent)
        {
            _agent = agent;
            _ai = _agent.GetComponent<AIController>();
        }

        public override void OnExit()
        {
            _agent.Idle();
        }

        public override void Tick()
        {
        }

        public override void FixedUpdate()
        {
            if (_movingRight && AtRightEdge())
                _movingRight = false;

            if (!_movingRight && AtLeftEdge())
                _movingRight = true;

            if (_movingRight)
                _agent.Move(_rightDirection);
            else
                _agent.Move(_leftDirection);
        }

        private bool AtLeftEdge()
        {
            return _agent.transform.position.x < _ai.LeftEdge.position.x;
        }

        private bool AtRightEdge()
        {
            return _agent.transform.position.x > _ai.RightEdge.position.x;
        }
    }
}
