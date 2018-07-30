using FSM;
using System.Collections;
using UnityEngine;

public class RollingState : State
{
    private int _baseLayer, _rollLayer;

    public RollingState(Agent agent) : base(agent)
    {
        _agent.Physics.IsImmobile = true;
    }

    public override void OnEnter()
    {
        _baseLayer = _agent.gameObject.layer;
        _rollLayer = LayerMask.NameToLayer("Roll");
        _agent.StartCoroutine(Roll());
        _agent.gameObject.layer = _rollLayer;
    }

    private IEnumerator Roll()
    {
        //_input.Agent.IsInvulnerable = true;
        _agent.Body.Roll();
        yield return new WaitForSeconds(0.5f);
        _agent.StateMachine.ChangeState(new GroundedState(_agent));
    }

    public override void Tick()
    {
        /*
        if (_input.Agent.IsDead)
            _input.ChangeState(new DeathState(_input));
            */
    }

    public override void OnExit()
    {
        //_input.Agent.IsInvulnerable = false;
        _agent.gameObject.layer = _baseLayer;
        _agent.StopCoroutine(Roll());
        _agent.Physics.IsImmobile = false;
    }
}
