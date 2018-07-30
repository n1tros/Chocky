using UnityEngine;

public class AgentPhysics
{
    private Rigidbody2D _rigidbody;
    private Transform _transform;
    private Agent _agent;

    public bool IsFalling { get; private set; }
    public bool IsImmobile { get; set; }

    private Transform _edgeDetection;

    public AgentPhysics(Transform transform, Rigidbody2D rigid, Agent agent)
    {
        _rigidbody = rigid;
        _transform = transform;
        _agent = agent;
        _agent.Body.OnJump += Jump;
        _agent.Body.OnRoll += Roll;
    }

    public void FixedTick()
    {
        IsFalling = _rigidbody.velocity.y < -0.1;

        if (!IsImmobile)
            Move(_agent.Body.MoveInput);
    }

    private void Move(float x)
    {
        FaceDirectionOfTravel(x);
        _rigidbody.velocity = new Vector3(x, _rigidbody.velocity.y, 0);
    }

    private void FaceDirectionOfTravel(float x)
    {
        if ((x > 0 && _transform.localScale.x < 0) || (x < 0 && _transform.localScale.x > 0))
            _transform.localScale = new Vector3(_transform.localScale.x * -1, _transform.localScale.y, _transform.localScale.z);
    }

    public void Jump()
    {
        _rigidbody.AddForce(new Vector2(0, _agent.Settings.JumpPower));
    }

    public void Roll()
    {
        _rigidbody.velocity = new Vector3(_agent.Settings.RollPower * _agent.transform.localScale.x, _rigidbody.velocity.y, 0);
    }

    public void Impact(float forceAmount, Vector2 Direction)
    {
        var knockback = new Vector2(Direction.x * forceAmount, 0);
        _rigidbody.AddForce(knockback, ForceMode2D.Impulse);
    }


}
    /*
    private Agent _agent;

    private int _baseLayer, _rollLayer;

    private void Awake()
    {
        _agent = GetComponent<Agent>();
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
        /*
        if (_agentController.IsDead)
            ZeroVelocity();

        _agentController.IsFalling = _rigidbody.velocity.y < -0.1;

        if(!_agentController.IsRolling) 
        Move(_agent.MoveInput);
        
            
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
    

}
*/