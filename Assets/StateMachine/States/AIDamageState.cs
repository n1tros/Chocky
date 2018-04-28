using System.Collections;
using UnityEngine;

namespace FSM
{
    public class AIDamageState : AIState
    {
        public AIDamageState(AgentController agentcontoller) : base(agentcontoller)
        {
        }

        public override void OnEnter()
        {
            _agentController.IsHit = true;
            _ai.StartCoroutine(ReturnToPreviousState(_agentController.AgentSettings.KnockBackTime));
        }
        
        private IEnumerator ReturnToPreviousState(float time)
        {
            yield return new WaitForSeconds(time);
            _agentController.IsHit = false;
            _ai.ChangeState(new AISearchState(_agentController));
        }

        public override void Tick()
        {
            _agentController.MoveInput = 0;
        }

        public override void OnExit()
        {
            _agentController.IsHit = false;
        }
    }
}
