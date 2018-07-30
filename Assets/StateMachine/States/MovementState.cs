using FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementState : State
{
    public MovementState(Agent agent) : base(agent)
    {
    }
    private void MoveAgent()
    {
        if (_brain.MoveInput > 0.1f || _brain.MoveInput < 0.1f)
        {
            _agentBody.MoveInput = _brain.MoveInput * _agentSettings.MoveSpeed;
        }
        else
        {
            _agentBody.MoveInput = 0;
        }
    }
}
