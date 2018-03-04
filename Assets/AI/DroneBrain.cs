using FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIBrain/Drone")]
public class DroneBrain : Brain
{
    [SerializeField]
    private float _timeToIdle = 0f;
    public override float IdleTransitionTime
    {
        get { return _timeToIdle; }
        internal set {}
    }

    [SerializeField]
    private float _timeToPatrol = 0f;
    public override float PatrolTransitionTime
    {
        get { return _timeToPatrol; }
        internal set {}
    }

    [SerializeField]
    private float _timeToSearch = 0f;
    public override float SearchTransitionTime
    {
        get { return _timeToSearch; }
        internal set { }
    }

    [SerializeField] private float _searchTime = 5f;
    [SerializeField] private float _turnDelay = 1f;
    [SerializeField] private float _attackDelay = 1f;

    private bool _attacking, _timerActive;
    private IEnumerator _transition;

    private void OnEnable()
    {
        _timerActive = false;
        _attacking = false;
        IdleTransitionTime = _timeToIdle;
    }

    public override void AttackPattern(AIController ai)
    {
        if (ai.Target == null)
            return;

        if (!_attacking)
            ai.StartCoroutine(Attack(ai));
    }

    IEnumerator Attack(AIController ai)
    {
        _attacking = true;
        yield return new WaitForSeconds(_attackDelay);
        ai.Agent.Attack();
        _attacking = false;
    }

    public override void DecideState(AIController ai)
    {
        if (ai.Agent.CurrentState == MovementStateType.Death)
        {
            Reset(ai);
            ai.Death();
            return;
        }

        if (_attacking)
        {
            Reset(ai);
            return;
        }
    }

    void Reset(AIController ai)
    {
    }

    public override void DefaultIdleTransition(AIController ai)
    {
        ai.Patrol();
    }

    public override void DefaultPatrolTransition(AIController ai)
    {
        ai.Idle();
    }

    public override void DefaultSearchTransition(AIController ai)
    {
        ai.Patrol();
    }
}
