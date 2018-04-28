using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName ="Agent/Player")]
public class AgentPlayer : AgentSettings
{
    //TODO: Refactor
    [SerializeField] private float _moveSpeed = 0f;
    [SerializeField] private float _jumpHeight = 0f;
    [SerializeField] private float _rollSpeed = 2f;
    [SerializeField] private float _rollTime = 1f;
    [SerializeField] private float _invulnerabilityTimer = 1f;
    [SerializeField] private float _switchWeaponDelay = 0.4f;
    [SerializeField] private float crouchSpeed = 2f;

    public override float MoveSpeed { get { return _moveSpeed; } }
    public override float CrouchSpeed { get { return crouchSpeed; } }
    public override float JumpHeight { get { return _jumpHeight; } }
    public override float RollSpeed { get { return _rollSpeed; } }
    public override float InvulnerabilityTime { get { return _invulnerabilityTimer; } }
    public override float RollTime { get { return _rollTime; } }
    public override float SwitchWeaponDelay { get { return _switchWeaponDelay; } }
}
