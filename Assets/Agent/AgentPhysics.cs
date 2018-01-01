using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Physically move the agent and provides interface to RigidBody
/// </summary>
[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(AgentController))]
public class AgentPhysics : MonoBehaviour
{
    private Rigidbody2D _rigidbody = null;
    private AgentController _agentController = null;

    private int _baseLayer, _rollLayer;

    private void Awake()
    {
        _agentController = GetComponent<AgentController>();
    }

    void Start ()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _baseLayer = gameObject.layer;
        _rollLayer = LayerMask.NameToLayer("Roll");
	}

    private void Move(float x)
    {
        if (x > 0)
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        else if (x < 0)
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);

        _rigidbody.velocity = new Vector3(x, _rigidbody.velocity.y, 0);
    }

    public void Jump(float jumpHeight)
    {
        _rigidbody.AddForce(new Vector2(0, jumpHeight));
    }

    public void FixedUpdate()
    {
        if (_agentController.CurrentState != MovementStateType.Roll && _rigidbody.velocity.y < -0.1)
            _agentController.AgentFall();

        if (_agentController.CurrentState == MovementStateType.Death)
            ZeroVelocity();

    }

    public void EndRoll()
    {
        gameObject.layer = _baseLayer;
    }

    public void Roll(float rollSpeed)
    {
        gameObject.layer = _rollLayer;
    }

    private void Idle()
    {
        _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
    }

    private void ZeroVelocity()
    {
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.constraints = RigidbodyConstraints2D.None;
    }
    
    private void OnEnable()
    {
        _agentController.OnMove += Move;
        _agentController.OnIdle += Idle;
        _agentController.OnJump += Jump;
        _agentController.OnDeath += ZeroVelocity;
        _agentController.OnRoll += Roll;
        _agentController.OnEndRoll += EndRoll;
    }

    private void OnDisable()
    {
        _agentController.OnMove -= Move;
        _agentController.OnIdle -= Idle;
        _agentController.OnJump -= Jump;
        _agentController.OnDeath -= ZeroVelocity;
        _agentController.OnRoll -= Roll;
        _agentController.OnEndRoll -= EndRoll;
    }
}
