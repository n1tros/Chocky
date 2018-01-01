using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName ="Agent/Player")]
public class AgentPlayer : Agent
{
    [SerializeField] private float _moveSpeed = 0f;
    [SerializeField] private float _jumpHeight = 0f;
    [SerializeField] private float _rollSpeed = 2f;
    
    public override float MoveSpeed { get { return _moveSpeed; } }
    public override float JumpHeight { get { return _jumpHeight; } }
    public override float RollSpeed { get { return _rollSpeed; } }
}
