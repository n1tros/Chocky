using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : PlayerMovementState
{
    public DeathState(PlayerInput input) : base(input)
    {
    }

    public override void Enter()
    {
        _input.Agent.Dead();   
    }

    public override void Tick()
    {
    }
}
