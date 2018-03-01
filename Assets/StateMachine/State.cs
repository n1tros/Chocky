namespace FSM
{
    public abstract class State
    {
        protected AgentController _agentController;

        public abstract void OnEnter(AgentController agent);
        public abstract void OnExit();
        public abstract void Tick();
        public abstract void FixedUpdate();
    }
}



