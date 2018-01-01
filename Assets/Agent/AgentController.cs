using FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Logic for changing player states and firing agent based events
/// </summary>

public class AgentController : MonoBehaviour
{
    [SerializeField] private Agent _agentType = null;
    [SerializeField] private bool _isInvulnerable = false;
    [SerializeField] private bool _isCollidable = true;
    //TODO: extract this to _agentType;
    [SerializeField] private float _rollTime = 1f;

    public delegate void Move(float x);
    public delegate void Jump(float x);
    public delegate void Idle();
    public delegate void Fall();
    public delegate void ToggleWeapon();
    public delegate void Attack();
    public delegate void DrawWeapon(bool weaponOut);
    public delegate void TakeDamage(float amount);
    public delegate void Death();
    public delegate void Roll(float rollSpeed);
    public delegate void EndRoll();

    public event Move OnMove;
    public event Idle OnIdle;
    public event Jump OnJump;
    public event Fall OnFall;
    public event ToggleWeapon OnToggleWeapon;
    public event Attack OnAttack;
    public event DrawWeapon OnDrawWeapon;
    public event TakeDamage OnTakeDamage;
    public event Death OnDeath;
    public event Roll OnRoll;
    public event EndRoll OnEndRoll;

    public MovementStateType CurrentState { get; set; }
    public MovementStateType PreviousState { get; set; }

    public bool IsGrounded { get; set; }
    public bool IsInvulnerable { get { return _isInvulnerable; } set { _isInvulnerable = value; } }

    
    private float _switchWeaponDelay = 0.2f;
    private bool _switchingWeapon, _rollTimerActive = false;

    private void Start()
    {
        AgentIdle();
        CurrentState = MovementStateType.Idle;
    }

    public void MoveAgent(float xDirection)
    {
        if (Immobile())
            return;

        if (AgentOnGround())
            ChangeState(MovementStateType.Movement);

        OnMove(xDirection * _agentType.MoveSpeed);
    }

    private bool Immobile()
    {
        return CurrentState == MovementStateType.Damage || CurrentState == MovementStateType.Death || CurrentState == MovementStateType.Roll;
    }

    public void AgentIdle()
    {

        if ((AgentOnGround() || CurrentState == MovementStateType.Start) && !Immobile())
        {
            ChangeState(MovementStateType.Idle);
            OnIdle();
        }
    }

    public void AgentJump()
    {
        if (CurrentState != MovementStateType.Jumping && 
            CurrentState != MovementStateType.Falling && 
            CurrentState != MovementStateType.Damage &&
            CurrentState != MovementStateType.Death &&
            CurrentState != MovementStateType.Roll)
            {
                IsGrounded = false;
                ChangeState(MovementStateType.Jumping);
                OnJump(_agentType.JumpHeight);
            }
    }

    public void AgentFall()
    {
        if ((CurrentState == MovementStateType.Jumping || CurrentState == MovementStateType.Roll || CurrentState == MovementStateType.Movement) && !AgentOnGround() )
        {
            ChangeState(MovementStateType.Falling);
            OnFall();
        }
    }

    public void AgentLand()
    {
        ChangeState(MovementStateType.Movement);
    }

    public void AgentToggleWeapon()
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

    public void AgentAttack()
    {
        if (CurrentState != MovementStateType.Death)
            OnAttack();
    }

    public bool AgentOnGround()
    {
        return IsGrounded && CurrentState != MovementStateType.Jumping && CurrentState != MovementStateType.Falling;
    }

    public void AgentDrawWeapon(bool weaponOut)
    {
        OnDrawWeapon(weaponOut);
    }

    public void AgentTakeDamage(float amount)
    {
        if (CurrentState != MovementStateType.Death && !IsInvulnerable)
        {
            ChangeState(MovementStateType.Damage);
            OnTakeDamage(amount);
        }
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

    public void AgentRoll()
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
        OnMove(_agentType.RollSpeed * transform.localScale.x);
        ChangeState(MovementStateType.Roll);
        OnRoll(_agentType.RollSpeed);
        yield return new WaitForSeconds(_rollTime);
        _rollTimerActive = false;
    }

    public void AgentEndRoll()
    {
        if (AgentOnGround())
        {
            if (PreviousState == MovementStateType.Movement)
                ChangeState(MovementStateType.Movement);
            else
            {
                ChangeState(MovementStateType.Idle);
                AgentIdle();
            }
        }
        else
        { AgentFall(); Debug.Log("End roll Fall"); }

        OnEndRoll();
        IsInvulnerable = false;
    }

    public void ChangeState(MovementStateType state)
    {
        if (CurrentState != state)
        {
            Debug.Log(gameObject.name + " now in " + state);
            PreviousState = CurrentState;
            CurrentState = state;
        }
    }

    private void FixedUpdate()
    {
        if (CurrentState == MovementStateType.Falling && IsGrounded)
        {
            AgentLand();
        }
    }
}
