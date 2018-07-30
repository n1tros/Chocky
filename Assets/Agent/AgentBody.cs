using System;
using UnityEngine;

public class AgentBody
{
    private float _moveInput;
    public float MoveInput
    {
        get { return _moveInput; }
        set { _moveInput = value; }
    }

    public event Action OnJump = delegate { };
    public event Action OnLand = delegate { };
    public event Action OnFall = delegate { };
    public event Action OnSwitchWeapons = delegate { };
    public event Action OnAttack = delegate { };
    public event Action OnMeleeAttack = delegate { };
    public event Action OnDrawWeapon = delegate { };
    public event Action OnDrawMelee = delegate { };
    public event Action OnTakeDamage = delegate { };
    public event Action OnDeath = delegate { };
    public event Action OnRoll = delegate { };
    public event Action OnEndRoll = delegate { };
    public event Action OnCrouch = delegate { };

    public void Jump()
    {
        OnJump();
    }

    public void Fall()
    {
        OnFall();
    }

    public void Land()
    {
        OnLand();
    }

    public void SwitchWeapons()
    {
        OnSwitchWeapons();
    }

    public void Attack()
    {
        OnAttack();
    }

    public void MeleeAttack()
    {
        OnMeleeAttack();
    }

    public void DrawWeapon()
    {
        OnDrawWeapon();
    }

    public void DrawMelee()
    {
        OnDrawMelee();
    }

    public void TakeDamage()
    {
        OnTakeDamage();
    }

    public void Dead()
    {
        OnDeath();
    }

    public void Roll()
    {
        OnRoll();
    }

    public void EndRoll()
    {
        OnEndRoll();
    }

    public void Crouch()
    {
        OnCrouch();
    }
}
