using UnityEngine;

[CreateAssetMenu(menuName = "Agent/AgentSettings")]
public class AgentSettings : ScriptableObject
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpPower;
    [SerializeField] private float _rollPower;
    [SerializeField] private float _invulnerabilityTimeWhenHit;
    [SerializeField] private float _rollInvulnerabilityTime;
    [SerializeField] private float _weaponSwitchDelay;
    [SerializeField] private float _crouchWalkingSpeed;
    [SerializeField] private int _xpOnDeath;
    [SerializeField] private bool _useAI;
    [SerializeField] private bool _useMeleeAsMain;

    public float MoveSpeed { get { return _moveSpeed; } }    
    public float JumpPower { get { return _jumpPower; } }
    public float RollPower { get { return _rollPower; } }
    public float InvulnerabilityTimeWhenHit { get { return _invulnerabilityTimeWhenHit; } }
    public float RollInvulnerabilityTime { get { return _rollInvulnerabilityTime; } }
    public float WeaponSwitchDelay { get { return _weaponSwitchDelay; } }
    public float CrouchWalkingSpeed { get { return _crouchWalkingSpeed; } }
    public bool UseAI { get { return _useAI; } }
    public bool UseMeleeAsMain { get { return _useMeleeAsMain; } }
    public int XpOnDeath { get { return _xpOnDeath; } }

}
