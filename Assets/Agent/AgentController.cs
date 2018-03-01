using FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Logic for changing player states and firing agent based events
/// </summary>

public class AgentController : MonoBehaviour
{
    [SerializeField] private AgentSettings _agentSettings = null;

    public event Action<float> OnMove = delegate { };
    public event Action<float> OnJump = delegate { };
    public event Action OnIdle = delegate { };
    public event Action OnFall = delegate { };
    public event Action OnToggleWeapon = delegate { };
    public event Action OnAttack = delegate { };
    public event Action<bool> OnDrawWeapon = delegate { };
    public event Action<float> OnTakeDamage = delegate { };
    public event Action OnDeath = delegate { };
    public event Action<float> OnRoll = delegate { };
    public event Action OnEndRoll = delegate { };
    
    public MovementStateType CurrentState { get; set; }
    public MovementStateType PreviousState { get; set; }

    public bool IsGrounded { get; set; }
    public bool IsInvulnerable { get; set; }
 
    private float _switchWeaponDelay = 0.2f;
    private bool _switchingWeapon, _rollTimerActive = false;

    private void Start()
    {
        Idle();
    }

    public void Idle()
    {
        if ((Grounded() || CurrentState == MovementStateType.Start) && !Immobile())
        {
            ChangeState(MovementStateType.Idle);
            OnIdle();
        }
    }

    public void Move (float direction)
    {
        if (Immobile())
            return;

        if (Grounded())
            ChangeState(MovementStateType.Movement);

        OnMove(direction * _agentSettings.MoveSpeed);
    }

    private bool Immobile()
    {
        return CurrentState == MovementStateType.Damage 
            || CurrentState == MovementStateType.Death 
            || CurrentState == MovementStateType.Roll;
    }

    public void Jump()
    {
        if (CanJump())
        {
            IsGrounded = false;
            ChangeState(MovementStateType.Jumping);
            OnJump(_agentSettings.JumpHeight);
        }
    }

    private bool CanJump()
    {
        return CurrentState != MovementStateType.Jumping 
            && CurrentState != MovementStateType.Falling 
            && !Immobile();
    }

    public void Fall()
    {
        if (InMotion() && !Grounded())
        {
            ChangeState(MovementStateType.Falling);
            OnFall();
        }
    }

    private bool InMotion()
    {
        return CurrentState == MovementStateType.Jumping
            || CurrentState == MovementStateType.Roll
            || CurrentState == MovementStateType.Movement;
    }

    public bool Grounded()
    {
        return IsGrounded 
            && CurrentState != MovementStateType.Jumping 
            && CurrentState != MovementStateType.Falling;
    }

    public void Land()
    {
        ChangeState(MovementStateType.Movement);
    }

    public void ToggleWeapon()
    {
        if (!_switchingWeapon && (CurrentState != MovementStateType.Damage && CurrentState != MovementStateType.Death))
            StartCoroutine(ToggleWeaponDelay());
    }

    IEnumerator ToggleWeaponDelay()
    {
        _switchingWeapon = true;
        yield return new WaitForSeconds(_switchWeaponDelay);             
        OnToggleWeapon();
        _switchingWeapon = false;
    }

    public void Attack()
    {
        if (CurrentState != MovementStateType.Death)
            OnAttack();
    }

    public void DrawWeapon(bool weaponOut)
    {
        OnDrawWeapon(weaponOut);
    }

    public void TakeDamage(float amount)
    {
        if (CurrentState != MovementStateType.Death && !IsInvulnerable)
        {
            ChangeState(MovementStateType.Damage);
            StartCoroutine(InvulnerableTimer(amount));
        }
    }

    IEnumerator InvulnerableTimer(float amount)
    {
        IsInvulnerable = true;
        OnTakeDamage(amount);
        yield return new WaitForSeconds(_agentSettings.InvulnerabilityTime);
        IsInvulnerable = false;
    }

    public void AgentPreviousState()
    {
        if(CurrentState != MovementStateType.Death)
            ChangeState(PreviousState);
    }

    public void AgentDead()
    {
        ChangeState(MovementStateType.Death);
        OnDeath();
    }

    public void Roll()
    {
        if (!Immobile() && !_rollTimerActive)
        {
            IsInvulnerable = true;
            StartCoroutine(RollTimer());
        }         
    }

    IEnumerator RollTimer()
    {
        _rollTimerActive = true;
        OnMove(_agentSettings.RollSpeed * transform.localScale.x);
        ChangeState(MovementStateType.Roll);
        OnRoll(_agentSettings.RollSpeed);
        yield return new WaitForSeconds(_agentSettings.RollTime);
        _rollTimerActive = false;
    }

    public void EndRoll()
    {
        if (Grounded())
        {
            if (PreviousState == MovementStateType.Movement)
                ChangeState(MovementStateType.Movement);
            else
            {
                ChangeState(MovementStateType.Idle);
                Idle();
            }
        }
        else
          Fall();

        OnEndRoll();
        IsInvulnerable = false;
    }

    public void ChangeState(MovementStateType state)
    {
        if (CurrentState != state)
        {
            PreviousState = CurrentState;
            CurrentState = state;
        }
    }

    private void FixedUpdate()
    {
        if (CurrentState == MovementStateType.Falling && IsGrounded)
        {
            Land();
        }
    }
}
