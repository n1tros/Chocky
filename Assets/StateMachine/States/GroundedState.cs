using FSM;
using UnityEngine;

public class GroundedState : State
{
    public GroundedState(Agent agent) : base(agent)
    {
    }

    public override void Tick()
    {
        base.Tick();
        if (_brain.Jump == true)
        {
            _stateMachine.ChangeState(new JumpState(_agent));
        }

        if (_brain.Crouch == true)
            _agent.StateMachine.ChangeState(new CrouchState(_agent));
    }
}
