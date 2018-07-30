namespace FSM
{
    public abstract class State
    {
        internal readonly Agent _agent;
        internal readonly Brain _brain;
        internal readonly StateMachine _stateMachine;
        internal readonly AgentBody _agentBody;
        internal readonly AgentSettings _agentSettings;

        public State(Agent agent)
        {
            _agent = agent;
            _brain = agent.Brain;
            _stateMachine = agent.StateMachine;
            _agentBody = agent.Body;
            _agentSettings = agent.Settings;
        }

        public virtual void OnEnter() {}
        public virtual void OnExit() {}
        public virtual void Tick()
        {
            if (_agent.Health.IsDead)
            {
                _agent.StateMachine.ChangeState(new DeathState(_agent));
                return;
            }

            MoveAgent();

            if (_brain.Roll == true && _brain.Attack != true)
                _agent.StateMachine.ChangeState(new RollingState(_agent));

            if (_brain.Attack == true)
            {
                _agentBody.Attack();
            }

            if (_brain.MeleeAttack)
                _agent.Body.MeleeAttack();

            if (_brain.SwitchWeapons == true)
            {
                _agentBody.SwitchWeapons();
            }

            if (_agent.Physics.IsFalling)
                _stateMachine.ChangeState(new FallState(_agent));
        }

        public virtual void FixedTick() {}

        private void MoveAgent()
        {
            if (_brain.MoveInput > 0.1f || _brain.MoveInput < 0.1f)
            {
                _agentBody.MoveInput = _brain.MoveInput * _agent.Stats.MoveSpeed;
            }
            else
            {
                _agentBody.MoveInput = 0;
            }
        }
    }
}



