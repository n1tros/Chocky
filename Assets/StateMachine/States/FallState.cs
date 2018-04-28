using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : PlayerMovementState
{
    public FallState(PlayerInput input) : base(input)
    {
    }

    public override void Enter()
    {
        _input.Agent.Fall();
        Debug.Log("Enter fall state");
    }

    public override void Tick()
    {
        base.Tick();

        if (_input.Agent.IsGrounded)
        {
            _input.ChangeState(new GroundedState(_input));
        }
    }
}
