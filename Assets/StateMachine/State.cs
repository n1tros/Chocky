namespace FSM
{
    public abstract class State
    {
        public abstract void OnEnter(AgentController agent);
        public abstract void OnExit();
        public abstract void Update();
        public abstract void FixedUpdate();
    }
}



