using FSM;
using UnityEngine;

public class FallState : State
{
    public FallState(Agent agent) : base(agent)
    {
    }

    public override void OnEnter()
    {
        _agent.Body.Fall();
    }

    public override void Tick()
    {
        if (_agent.GroundCheck.IsGrounded)
            _agent.StateMachine.ChangeState(new GroundedState(_agent));
        else if (!_agent.Physics.IsFalling)
            _agent.StateMachine.ChangeState(new JumpState(_agent));

        base.Tick();
    }

    public override void OnExit()
    {
        _agent.Body.Land();
    }
}
