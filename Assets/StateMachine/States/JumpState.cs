using FSM;
using UnityEngine;

public class JumpState : State
{
    public JumpState(Agent agent) : base(agent)
    {
    }

    public override void OnEnter()
    {
        _agent.GroundCheck.IsGrounded = false;
        _agentBody.Jump();
    }

    public override void Tick()
    {
        if (_agent.GroundCheck.IsGrounded)
        {
            _stateMachine.ChangeState(new GroundedState(_agent));
            return;
        }

        base.Tick();
    }

    public override void OnExit()
    {
    }
}
