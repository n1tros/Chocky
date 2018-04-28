namespace FSM
{
    internal class AIAttackState : AIState
    {
        public AIAttackState(AgentController agentcontoller) : base(agentcontoller)
        {
        }

        public override void OnEnter()
        {
            _ai.InAttackState = true;
            var weaponController = _ai.GetComponent<WeaponController>();
            if (weaponController.Current.IsMelee)
                _ai.ChangeState(new AIMeleeAttackState(_agentController));
            else
                _ai.ChangeState(new AIRangedAttackState(_agentController));
        }
    }
}