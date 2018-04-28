namespace FSM
{
    public abstract class State
    {
        protected AgentController _agentController;

        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void Tick() { }
        public virtual void FixedTick() { }

        public State (AgentController agentcontoller)
        {
            _agentController = agentcontoller;
        }
    }
}



