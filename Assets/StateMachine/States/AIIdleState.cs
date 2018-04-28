using System.Collections;
using UnityEngine;

namespace FSM
{
    public class AIIdleState : AIState
    {
        public AIIdleState(AgentController agentcontoller) : base(agentcontoller)
        {
        }

        public override void OnEnter()
        {
            _agentController.MoveInput = 0;
            _ai.StartCoroutine(TransitionToDefaultState());
        }

        IEnumerator TransitionToDefaultState()
        {
            yield return new WaitForSeconds(_ai.CurrentBrain.IdleTransitionTime);
            _ai.CurrentBrain.DefaultIdleTransition(_ai);
        }

        public override void Tick()
        {
            base.Tick();
            _agentController.MoveInput = 0;
        }

        public override void OnExit()
        {
            _ai.StopCoroutine(TransitionToDefaultState());
        }
    }
}

