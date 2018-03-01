using UnityEngine;

public abstract class AgentSettings : ScriptableObject
{
    public abstract float MoveSpeed { get; }    
    public abstract float JumpHeight { get; }
    public abstract float RollSpeed { get; }
    public abstract float InvulnerabilityTime { get; }
    public abstract float RollTime { get; }
}
