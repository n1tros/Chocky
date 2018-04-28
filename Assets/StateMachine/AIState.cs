namespace FSM
{
    public abstract class AIState : State
    {
        protected AIController _ai;

        public AIState(AgentController agentcontoller) : base(agentcontoller)
        {
            _ai = agentcontoller.GetComponent<AIController>();
        }

        public override void Tick()
        {
            if (_agentController.IsDead)    
            {
                _ai.ChangeState(new AIDeathState(_agentController));
                return;
            }

            if (_agentController.IsHit)
            {
                _ai.ChangeState(new AIDamageState(_agentController));
                return;
            }

            if (_ai.Target != null && !_ai.InAttackState)
            {
                _ai.ChangeState(new AIAttackState(_agentController));
            }
        }
    }
}



