using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIBrain/Drone")]
public class DroneBrain : Brain
{
    [SerializeField] private float _timeToIdle = 0f;
    [SerializeField] private float _timeToPatrol = 0f;
    [SerializeField] private float _searchTime = 5f;
    [SerializeField] private float _turnDelay = 1f;
    [SerializeField] private float _attackDelay = 1f;

    private bool _attacking, _timerActive;

    private void OnEnable()
    {
        _timerActive = false;
        _attacking = false;
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
        ai.Agent.AgentAttack();
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

        if (ai.CurrentState == AIStateType.AISearch && !_timerActive)
            ai.StartCoroutine(Transition(_searchTime, ai, AIStateType.AISearch));

        if (ai.CurrentState == AIStateType.AIIdle && !_timerActive)
            ai.StartCoroutine(Transition(_timeToIdle, ai, AIStateType.AIPatrol));

        if (ai.CurrentState == AIStateType.AIPatrol && !_timerActive)
            ai.StartCoroutine(Transition(_timeToPatrol, ai, AIStateType.AIIdle));

        else if (ai.CurrentState == AIStateType.AIStart)
            ai.Patrol();
    }

    IEnumerator Transition(float time, AIController ai, AIStateType toState)
    {
        _timerActive = true;
        if (toState == AIStateType.AIPatrol || toState == AIStateType.AISearch)
        {
            yield return new WaitForSeconds(time);
            ai.Patrol();
        }
        else if (toState == AIStateType.AIIdle)
        {
            yield return new WaitForSeconds(time);
            ai.Idle();
        }
        else
            yield return new ArgumentException("error no such state");
    }

    void Reset(AIController ai)
    {
        ai.StopCoroutine("Transition");
        _timerActive = false;
    }
}
