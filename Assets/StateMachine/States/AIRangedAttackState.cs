namespace FSM
{
    public class AIRangedAttackState : AIState
    {
        public AIRangedAttackState(AgentController agentcontoller) : base(agentcontoller)
        {
        }

        public override void OnEnter()
        {
            _agentController.DrawWeapon(true);
        }

        public override void Tick()
        {
            base.Tick();

            if (_ai.Target == null)
            {
                _ai.ChangeState(new AISearchState(_agentController));
            }
            else
            {
                _agentController.MoveInput = 0;
                _ai.CurrentBrain.AttackPattern(_ai);
            }
        }

        public override void OnExit()
        {
            _ai.InAttackState = false;
        }
    }
}

