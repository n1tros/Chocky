using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : PlayerMovementState
{
    public GroundedState(PlayerInput input) : base(input)
    {
    }

    public override void Enter()
    {
        Debug.Log("Enter state 1");
    }

    public override void Tick()
    {
        base.Tick();

        if (_input.Crouch())
            _input.ChangeState(new CrouchState(_input));

        if (_input.Jump())
        {
            _input.ChangeState(new JumpState(_input));
        }
    }
}
