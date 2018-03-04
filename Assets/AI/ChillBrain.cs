using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIBrain/Chill")]
public class ChillBrain : Brain
{
    public override void AttackPattern(AIController ai)
    {

    }

    public override void DecideState(AIController ai)
    {
        ai.Idle();
    }

    public override void DefaultIdleTransition(AIController ai)
    {
        throw new NotImplementedException();
    }

    public override void DefaultPatrolTransition(AIController ai)
    {
        throw new NotImplementedException();
    }

    public override void DefaultSearchTransition(AIController ai)
    {
        throw new NotImplementedException();
    }
}
