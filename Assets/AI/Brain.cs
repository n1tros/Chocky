using FSM;
using UnityEngine;

public abstract class Brain : ScriptableObject
{
    [SerializeField]
    private float _timeToIdle = 0f;
    public float IdleTransitionTime
    {
        get { return _timeToIdle; }
    }

    [SerializeField]
    private float _timeToPatrol = 0f;
    public float PatrolTransitionTime
    {
        get { return _timeToPatrol; }
    }

    [SerializeField]
    private float _timeToSearch = 0f;
    public float SearchTransitionTime
    {
        get { return _timeToSearch; }
    }

    [SerializeField]
    private float _searchStateTurnTime;
    public float SearchStateTurnTime
    {
        get { return _searchStateTurnTime; }
    }

    [SerializeField] protected float _attackDelay = 1f;

    public abstract void DecideState(AIController ai);
    public abstract void AttackPattern(AIController ai);

    public abstract void DefaultIdleTransition(AIController ai);
    public abstract void DefaultPatrolTransition(AIController ai);
    public abstract void DefaultSearchTransition(AIController ai);
}
