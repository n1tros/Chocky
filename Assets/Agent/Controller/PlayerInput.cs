using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool IsGrounded { get; set; }

    public AgentController Agent { get; private set; }

    private void Awake()
    {
        Agent = GetComponent<AgentController>();
        ChangeState(new GroundedState(this));
    }

    private void Update()
    {
        IsGrounded = Agent.IsGrounded;
        Agent.CurrentStateTest.Tick();
    }

    public void ChangeState(PlayerMovementState state)
    {
        if (Agent.CurrentStateTest == null)
        {
            Agent.CurrentStateTest = state;
            Agent.CurrentStateTest.Enter();
        }

        if (Agent.CurrentStateTest.GetType().Name != state.GetType().Name)
        {
            Agent.CurrentStateTest.Exit();
            Agent.CurrentStateTest = state;
            Agent.CurrentStateTest.Enter();
        }

    }

    public float Movement()
    {
        return Input.GetAxisRaw(StringReference.Horizontal);
    }

    public bool Attack()
    {
        return Input.GetButtonDown(StringReference.Fire1);
    }

    public bool Jump()
    {
        return Input.GetButtonDown(StringReference.Jump);
    }

    public bool ToggleWeapons()
    {
        return Input.GetButtonDown(StringReference.WeaponToggle);
    }

    public bool Roll()
    {
        return Input.GetButton(StringReference.Fire3);
    }

    public bool Crouch()
    {
        return Input.GetButton(StringReference.Fire2);
    }

    public bool IsMoving()
    {
        if (Movement() < -0.1f || Movement() > 0.1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
