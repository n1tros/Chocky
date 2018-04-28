namespace FSM
{
    public class AIStartState : AIState
    {
        public AIStartState(AgentController agentcontoller) : base(agentcontoller)
        {
        }

        public override void OnEnter()
        {
            _ai.StateMachine.ChangeState(new AIIdleState(_agentController));
        }
    }
}

