using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private AgentController _agent = null;

    private void Start()
    {
        _agent = GetComponent<AgentController>();
    }

    private void Update()
    {
        if (Input.GetButtonDown(StringReference.Fire1))
            _agent.Attack();

        if (Input.GetButtonDown(StringReference.Jump))
            _agent.Jump();

        if (Input.GetButtonDown(StringReference.WeaponToggle))
            _agent.ToggleWeapon();
        
        if (Input.GetButton(StringReference.Fire3))
            _agent.Roll();
    }

    private void FixedUpdate()
    {
        var xInput = Input.GetAxisRaw(StringReference.Horizontal);
        if ((xInput > -0.1 && xInput < 0.1) && _agent.IsGrounded)
            _agent.Idle();
        else
            _agent.Move(xInput);

        if (Input.GetAxisRaw("dPadX") < -0.5f)
            _agent.ToggleWeapon();
    }
}
