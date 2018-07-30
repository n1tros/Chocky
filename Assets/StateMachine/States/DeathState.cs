using FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State
{
    public DeathState(Agent agent) : base(agent)
    {
    }

    public override void OnEnter()
    {
        _agent.Body.Dead();
        _agent.Physics.IsImmobile = true;
    }

    public override void Tick()
    {
        
    }

    public override void OnExit()
    {
        _agent.Physics.IsImmobile = false;
    }
}
