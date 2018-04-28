using System.Collections;
using UnityEngine;
using FSM;

[CreateAssetMenu(menuName = "AIBrain/Drone")]
public class DroneBrain : Brain
{
    private void OnEnable()
    {
    }

    public override void AttackPattern(AIController ai)
    {
        if (ai.Target != null && !ai.IsAttacking)
            ai.StartCoroutine(Attack(ai));
    }

    IEnumerator Attack(AIController ai)
    {
        ai.IsAttacking = true;
        yield return new WaitForSeconds(_attackDelay);
        if (!ai._isMelee)
        {
            ai.Agent.Attack();
        }
        else
        {
            ai.Agent.MeleeAttack();
        }
        ai.IsAttacking = false;
    }

    public override void DecideState(AIController ai)
    {
    }

    void Reset(AIController ai)
    {
    }

    public override void DefaultIdleTransition(AIController ai)
    {
        ai.ChangeState(new AIPatrolState(ai.Agent));
    }

    public override void DefaultPatrolTransition(AIController ai)
    {
        ai.ChangeState(new AIIdleState(ai.Agent));
    }

    public override void DefaultSearchTransition(AIController ai)
    {
        ai.ChangeState(new AIPatrolState(ai.Agent));
    }
}
