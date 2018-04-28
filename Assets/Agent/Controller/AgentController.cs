using System;
using UnityEngine;

/// <summary>
/// Fire Agent based Movement events
/// </summary>
public class AgentController : MonoBehaviour
{
    [SerializeField]
    private AgentSettings _agentSettings = null;
    public AgentSettings AgentSettings { get { return _agentSettings; } }

    [SerializeField]
    private Stats _playerStats;
    public Stats PlayerStats { get { return _playerStats; } }

    [SerializeField]
    private AbilityMap _abilityMap;
    public AbilityMap AbilityMap { get { return _abilityMap; } }

    private float _moveInput;
    public float MoveInput
    {
        get { return _moveInput; }
        set
        {
            if (IsCrouched == true)
                _moveInput = value * _agentSettings.CrouchSpeed;
            else
                _moveInput = value * _agentSettings.MoveSpeed;
        }
    }

    public event Action<float> OnJump = delegate { };
    public event Action OnLand = delegate { };
    public event Action OnFall = delegate { };
    public event Action<float> OnToggleWeapon = delegate { };
    public event Action OnAttack = delegate { };
    public event Action OnMeleeAttack = delegate { };
    public event Action<bool> OnDrawWeapon = delegate { };
    public event Action<float> OnTakeDamage = delegate { };
    public event Action OnDeath = delegate { };
    public event Action<float> OnRoll = delegate { };
    public event Action OnEndRoll = delegate { };

    public bool IsGrounded { get; set; }
    public bool IsInvulnerable { get; set; }
    public bool IsFalling { get; set; }
    public bool IsJumping { get; set; }
    public bool IsDead { get; set; }
    public bool IsRolling { get; set; }
    public bool IsInAir { get { return IsFalling || IsJumping; }}
    public bool IsCrouched { get; set; }
    public bool IsAttacking { get; set; }
    public bool IsHit { get; set; }

    public PlayerMovementState CurrentStateTest { get; set; }

    public void Jump()
    {
        OnJump(_agentSettings.JumpHeight);
    }

    public void Fall()
    {
        if (!IsGrounded)
        {
            OnFall();
        }
    }

    public void Land()
    {
        OnLand();
    }

    public void ToggleWeapon()
    {
        OnToggleWeapon(_agentSettings.SwitchWeaponDelay);
    }

    public void Attack()
    {
        if (!IsDead && !IsAttacking)
            OnAttack();
    }

    public void MeleeAttack()
    {
        if (!IsDead && !IsAttacking)
            OnMeleeAttack();
    }

    public void DrawWeapon(bool weaponOut)
    {
        OnDrawWeapon(weaponOut);
    }

    public void TakeDamage(float amount)
    {
        if (!IsInvulnerable)
        {
            IsHit = true;
            OnTakeDamage(amount);
        }
    }

    public void Dead()
    {
        IsDead = true;
        OnDeath();
    }

    public void Roll()
    {
        OnRoll(_agentSettings.RollSpeed);
    }

    public void EndRoll()
    {
        OnEndRoll();
    }
}
