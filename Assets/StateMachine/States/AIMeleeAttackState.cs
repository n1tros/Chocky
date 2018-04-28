namespace FSM
{
    public class AIMeleeAttackState : AIState
    {
        public AIMeleeAttackState(AgentController agentcontoller) : base(agentcontoller)
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
                _ai.ChangeState(new AISearchState(_agentController));
            else
                AttackOrCloseRange();
        }

        private void AttackOrCloseRange()
        {
            if (_ai.TargetInMeleeRange)
            {
                _agentController.MoveInput = 0;
                _agentController.Attack();
            }
            else
                _agentController.MoveInput = _agentController.transform.position.x < _ai.Target.transform.position.x ? 1 : -1;
        }

        public override void OnExit()
        {
            _ai.InAttackState = false;
        }
    }
}

