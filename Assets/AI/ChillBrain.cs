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
}
