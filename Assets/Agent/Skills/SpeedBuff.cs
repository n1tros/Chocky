using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpeedBuff : Skill
{
    [SerializeField]
    private float _speedModifier;

    public override void AddSkill(Agent agent)
    {
        agent.Stats.MoveSpeed *= _speedModifier;
    }
}
