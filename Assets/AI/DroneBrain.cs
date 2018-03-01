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
    private IEnumerator _transition;

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

        if (ai.CurrentState == AIStateType.AISearch && !_timerActive)
            ai.StartCoroutine(Transition(_searchTime, ai, AIStateType.AIPatrol));

        if (ai.CurrentState == AIStateType.AIIdle && !_timerActive)
            ai.StartCoroutine(Transition(_timeToIdle, ai, AIStateType.AIPatrol));

        if (ai.CurrentState == AIStateType.AIPatrol && !_timerActive)
        {
            Debug.Log("AI going frpom Patrol to Idle");
            ai.StartCoroutine(Transition(_timeToPatrol, ai, AIStateType.AIIdle));
        }

        else if (ai.CurrentState == AIStateType.AIStart)
            ai.Patrol();
    }

    IEnumerator Transition(float time, AIController ai, AIStateType toState)
    {
        _timerActive = true;
        switch (toState)
        {
            case AIStateType.AIStart:
                break;
            case AIStateType.AIPatrol:
                yield return new WaitForSeconds(time);
                ai.Patrol();
                break;
            case AIStateType.AIChase:
                break;
            case AIStateType.AICombat:
                break;
            case AIStateType.AIIdle:
                yield return new WaitForSeconds(time);
                ai.Idle();
                break;
            case AIStateType.AIMeleeAttack:
                break;
            case AIStateType.AIRangedAttack:
                break;
            case AIStateType.AIDeath:
                break;
            case AIStateType.AIDamage:
                break;
            case AIStateType.AISearch:
                yield return new WaitForSeconds(time);
                ai.Search();
                break;
            default:
                yield return new ArgumentException("error no such state");
                break;
        }           
    }

    void Reset(AIController ai)
    {
        ai.StopCoroutine("Transition");
        _timerActive = false;
    }
}
