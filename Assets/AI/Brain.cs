using FSM;
using UnityEngine;

public abstract class Brain : ScriptableObject
{
    public virtual float IdleTransitionTime { get; internal set; }
    public virtual float PatrolTransitionTime { get; internal set; }
    public virtual float SearchTransitionTime { get; internal set; }

    public abstract void DecideState(AIController ai);
    public abstract void AttackPattern(AIController ai);

    public abstract void DefaultIdleTransition(AIController ai);
    public abstract void DefaultPatrolTransition(AIController ai);
    public abstract void DefaultSearchTransition(AIController ai);

}
