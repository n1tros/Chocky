﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

[CreateAssetMenu(menuName = "AIBrain/AttackTest")]
public class AttackTestBrain : Brain
{
    [SerializeField] private AgentController _target;

    public override void AttackPattern(AIController ai)
    {

    }

    public override void DecideState(AIController ai)
    {
        ai.Patrol();

        ai.TargetAgent(_target);
    }
}
