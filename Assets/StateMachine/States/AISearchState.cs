using System.Collections;
using UnityEngine;

namespace FSM
{
    public class AISearchState : AIState
    {
        public AISearchState(AgentController agentcontoller) : base(agentcontoller)
        {
        }

        public override void OnEnter()
        {
            _ai.StartCoroutine(TurnDelay());
            _ai.StartCoroutine(Transition());
        }

        public override void Tick()
        {
            base.Tick();
        }

        private IEnumerator Transition()
        {
            yield return new WaitForSeconds(_ai.CurrentBrain.SearchTransitionTime);
            _ai.CurrentBrain.DefaultSearchTransition(_ai);
        }

        IEnumerator TurnDelay()
        {
            while (true)
            {
                _agentController.MoveInput = 0;
                yield return new WaitForSeconds(2f);
                _agentController.transform.localScale = new Vector3(_agentController.transform.localScale.x * -1, 1, 1);
            }
        }

        public override void OnExit()
        {
            _ai.StopCoroutine(TurnDelay());
            _ai.StopCoroutine(Transition());
        }
    }
}

