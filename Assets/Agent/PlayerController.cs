﻿using System;
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
            _agent.AgentAttack();

        if (Input.GetButtonDown(StringReference.Jump))
            _agent.AgentJump();

        if (Input.GetButtonDown(StringReference.WeaponToggle))
            _agent.AgentToggleWeapon();
        
        if (Input.GetButton(StringReference.Fire3))
            _agent.AgentRoll();
    }

    private void FixedUpdate()
    {
        var xInput = Input.GetAxisRaw(StringReference.Horizontal);
        if ((xInput > -0.1 && xInput < 0.1) && _agent.IsGrounded)
            _agent.AgentIdle();
        else
            _agent.MoveAgent(xInput);

        if (Input.GetAxisRaw("dPadX") < -0.5f)
            _agent.AgentToggleWeapon();
    }
}