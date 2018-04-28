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
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start ()
    {
        _baseLayer = gameObject.layer;
        _rollLayer = LayerMask.NameToLayer("Roll");
	}

    private void Move(float x)
    {
        FaceDirectionOfTravel(x);
        if (x > -0.1 && x < 0.1)
        {
            Idle();
            return;
        }
        _rigidbody.velocity = new Vector3(x, _rigidbody.velocity.y, 0);
    }

    private void FaceDirectionOfTravel(float x)
    {
        if (x > 0)
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        else if (x < 0)
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
    }

    public void Jump(float jumpHeight)
    {
        _rigidbody.AddForce(new Vector2(0, jumpHeight));
    }

    public void FixedUpdate()
    {
        if (_agentController.IsDead)
            ZeroVelocity();

        _agentController.IsFalling = _rigidbody.velocity.y < -0.1;

        if(!_agentController.IsRolling)
            Move(_agentController.MoveInput);
    }

    public bool IsFalling()
    {
        return _rigidbody.velocity.y < -0.1;
    }

    public void EndRoll()
    {
        gameObject.layer = _baseLayer;
    }

    public void Roll(float rollSpeed)
    {
        _rigidbody.velocity = new Vector3(rollSpeed * transform.localScale.x, _rigidbody.velocity.y, 0);
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
        _agentController.OnJump += Jump;
        _agentController.OnDeath += ZeroVelocity;
        _agentController.OnRoll += Roll;
        _agentController.OnEndRoll += EndRoll;
    }

    private void OnDisable()
    {
        _agentController.OnJump -= Jump;
        _agentController.OnDeath -= ZeroVelocity;
        _agentController.OnRoll -= Roll;
        _agentController.OnEndRoll -= EndRoll;
    }
}
