using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName ="Agent/Player")]
public class AgentPlayer : AgentSettings
{
    [SerializeField] private float _moveSpeed = 0f;
    [SerializeField] private float _jumpHeight = 0f;
    [SerializeField] private float _rollSpeed = 2f;
    [SerializeField] private float _rollTime = 1f;
    [SerializeField] private float _invulnerabilityTimer = 1f;


    public override float MoveSpeed { get { return _moveSpeed; } }
    public override float JumpHeight { get { return _jumpHeight; } }
    public override float RollSpeed { get { return _rollSpeed; } }
    public override float InvulnerabilityTime { get { return _invulnerabilityTimer; } }
    public override float RollTime { get { return _rollTime; } }

}
