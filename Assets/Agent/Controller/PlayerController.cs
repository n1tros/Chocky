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

        if (Input.GetAxisRaw("dPadX") < -0.5f)
            _agent.ToggleWeapon();
    }
}
