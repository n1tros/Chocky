using UnityEngine;

/// <summary>
/// Base class for any agents in the game
/// </summary>
public abstract class Agent : ScriptableObject
{
    public abstract float MoveSpeed { get; }    
    public abstract float JumpHeight { get; }
    public abstract float RollSpeed { get; }
}
