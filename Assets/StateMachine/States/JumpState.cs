using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : PlayerMovementState
{
    public JumpState(PlayerInput input) : base(input)
    {
    }

    public override void Enter()
    {
        _input.Agent.Jump();
        _input.Agent.IsJumping = true;
        Debug.Log("Entering Jump state");
    }

    public override void Tick()
    {
        base.Tick();

        if (_input.Agent.IsFalling)
            _input.ChangeState( new FallState(_input));
    }

    public override void Exit()
    {
        _input.Agent.IsJumping = false;
    }
}
