using UnityEngine;

public abstract class Brain : ScriptableObject
{
    public abstract void DecideState(AIController ai);
    public abstract void AttackPattern(AIController ai);
}
