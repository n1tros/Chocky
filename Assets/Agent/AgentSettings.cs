using UnityEngine;

public abstract class AgentSettings : ScriptableObject
{
    public abstract float MoveSpeed { get; }    
    public abstract float JumpHeight { get; }
    public abstract float RollSpeed { get; }
    public abstract float InvulnerabilityTime { get; }
    public abstract float RollTime { get; }
    public abstract float SwitchWeaponDelay { get; }
    public virtual float CrouchSpeed { get; set; }
    public virtual float KnockBackTime { get; set; }
}
