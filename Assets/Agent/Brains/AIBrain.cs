using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "AI Brain - Move from A to B")]
public class AIBrain : Brain
{
    private EdgeCheck _edgeCheck;
    private Eyesight _eyes;
    private bool _turning;
    private bool _movingRight = false;
    private float _rightDirection = 1;
    private float _leftDirection = -1;
    private Agent _agent;
    private bool _searching;

    public override void SetupBrain(Agent agent)
    {
        _agent = agent;
        _eyes = _agent.GetComponentInChildren<Eyesight>();
        _edgeCheck = _agent.GetComponent<EdgeCheck>();
        Attack = false;
        MoveInput = 0;
        _movingRight = true;
        _turning = false;
        _searching = false;
        DeathManager.Enemies.Add(agent);
    }

    public override void ReadBrain()
    {
        if (_agent.Health.IsDead)
        {
            MoveInput = 0;
            DeathManager.ProcessDeath(_agent);
            _agent.gameObject.SetActive(false);
            return;
        }

        if (_eyes.HasTarget == true)
        {
            MoveInput = 0;
            AttackTarget(_eyes.Target);
            return;
        }
        else if (_eyes.LostTarget)
        {
            Attack = false;
            if (!_searching)
                _agent.StartCoroutine(SearchState());
            return;
        }

        Attack = false;
        if (_edgeCheck.EdgeHit && !_turning)
        {
            AtEdgeOfPlatform();
        }
        PatrolBetweenTwoPoints();
        
    }

    private void AttackTarget(Agent target)
    {
        MoveInput = _agent.transform.position.x < target.transform.position.x ? -1 : 1;
        MoveInput = 0;
        Attack = true;
    }

    private void PatrolBetweenTwoPoints()
    {
        if (_movingRight)
            MoveInput = _rightDirection;
        else if (!_movingRight)
            MoveInput = _leftDirection;
    }

    public void AtEdgeOfPlatform()
    {
        _agent.StartCoroutine(TurnTimer());
    }

    private IEnumerator SearchState()
    {
        _searching = true;
        yield return new WaitForSeconds(2f);

        if (_agent.transform.localScale.x >= 0)
        {
            _agent.transform.localScale = new Vector3(_leftDirection, _agent.transform.localScale.y, _agent.transform.localScale.z);
            //MoveInput = 0;
        }
        else
        {
            _agent.transform.localScale = new Vector3(_rightDirection, _agent.transform.localScale.y, _agent.transform.localScale.z);
            //MoveInput = 0;
        }
        _searching = false;
    }

    private IEnumerator TurnTimer()
    {
        _turning = true;
        _movingRight = !_movingRight;
        yield return new WaitForSeconds(1f);
        _turning = false;
    }
}
