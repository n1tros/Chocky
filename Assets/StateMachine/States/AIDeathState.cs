using UnityEngine;

namespace FSM
{
    public class AIDeathState : AIState
    {
        int _deathLayer = 17;

        public AIDeathState(AgentController agentcontoller) : base(agentcontoller)
        {
        }

        public override void OnEnter()
        {
            _ai.StopAllCoroutines();

            //TODO: Shutdown box colliders and move this into a "death" method.
            _agentController.MoveInput = 0;
            _agentController.Dead();

            _agentController.DrawWeapon(false);
            _agentController.gameObject.layer = _deathLayer;

            _agentController.GetComponent<Rigidbody2D>().isKinematic = true;
        }

      
    }
}


