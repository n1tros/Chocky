
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingState : PlayerMovementState
{
    IEnumerator _roll;

    public RollingState(PlayerInput input) : base(input)
    {
    }

    public override void Enter()
    {
        if (_roll == null)
        {
            _roll = Roll();
            _input.StartCoroutine(_roll);
        }
    }

    private IEnumerator Roll()
    {
        _input.Agent.IsInvulnerable = true;
        _input.Agent.IsRolling = true;
        _input.Agent.Roll();
        //TODO: need a way to get the rolling timer in here.
        yield return new WaitForSeconds(0.5f);
        _input.ChangeState(new GroundedState(_input));
    }

    public override void Tick()
    {
        if (_input.Agent.IsDead)
            _input.ChangeState(new DeathState(_input));
    }

    public override void Exit()
    {
        _input.Agent.IsRolling = false;
        _input.Agent.IsInvulnerable = false;
        _input.StopCoroutine(Roll());
    }
}
