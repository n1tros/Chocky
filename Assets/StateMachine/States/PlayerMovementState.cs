using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerMovementState
{
    protected PlayerInput _input;

    public PlayerMovementState(PlayerInput input)
    {
        _input = input;
    }

    public virtual void Enter() { }

    public virtual void Tick()
    {
        if (_input.Agent.IsDead)
        {
            _input.ChangeState(new DeathState(_input));
            return;
        }

        if (_input.Agent.IsHit)
        {
            _input.ChangeState(new DamagedState(_input));
            return;
        }

        _input.Agent.MoveInput = _input.Movement();

        if (_input.Attack())
            _input.Agent.Attack();

        if (_input.Roll())
            _input.ChangeState(new RollingState(_input));

        if (_input.ToggleWeapons() == true)
            _input.Agent.ToggleWeapon();
    }

    public virtual void FixedTick() { }

    public virtual void Exit() { }
}
