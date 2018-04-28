using System.Collections;
using UnityEngine;

public class DamagedState : PlayerMovementState
{
    public DamagedState(PlayerInput input) : base(input)
    {
    }

    public override void Enter()
    {
        _input.Agent.IsHit = true;
        _input.StartCoroutine(DamageCountdown());
    }

    IEnumerator DamageCountdown()
    {
        yield return new WaitForSeconds(_input.Agent.AgentSettings.InvulnerabilityTime);
        _input.ChangeState(new GroundedState(_input));
    }

    public override void Exit()
    {
        _input.StopCoroutine(DamageCountdown());
        _input.Agent.IsHit = false;
    }

    public override void Tick() {}
    public override void FixedTick() {}
}
