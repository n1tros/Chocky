using System.Collections;
using UnityEngine;
using FSM;
using System;

public class DamagedState : State
{
    public DamagedState(Agent agent) : base(agent)
    {
    }

    public override void OnEnter()
    {
        _agent.StartCoroutine(DamagedTime());
        _agent.Body.TakeDamage();
        _agent.Physics.IsImmobile = true;
    }

    public override void Tick()
    {
    }

    private IEnumerator DamagedTime()
    {
        yield return new WaitForSeconds(_agent.Settings.InvulnerabilityTimeWhenHit);
        _agent.StateMachine.ChangeState(new GroundedState(_agent));
    }

    public override void OnExit()
    {
        _agent.Physics.IsImmobile = false;
    }
}