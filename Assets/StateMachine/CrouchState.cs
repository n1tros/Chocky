using System;
using FSM;
public class CrouchState : State
{
    public CrouchState(Agent agent) : base(agent)
    {
    }

    public override void OnEnter()
    {
        _agent.Body.Crouch();
    }

    public override void Tick()
    {
        MoveAgent();

        if (_brain.Roll == true)
            _agent.StateMachine.ChangeState(new RollingState(_agent));

        if (_brain.Jump == true)
        {
            _stateMachine.ChangeState(new JumpState(_agent));
        }

        if (_brain.Crouch == false)
            _stateMachine.ChangeState(new GroundedState(_agent));

        if (_brain.Attack == true)
        {
            _agentBody.Attack();
        }
    }

    private void MoveAgent()
    {
        if (_brain.MoveInput > 0.1f || _brain.MoveInput < 0.1f)
        {
            _agentBody.MoveInput = _brain.MoveInput * _agentSettings.CrouchWalkingSpeed;
        }
        else
        {
            _agentBody.MoveInput = 0;
        }
    }
}
