using Spine.Unity;
using UnityEngine;
using Spine;

/// <summary>
/// Provides top layer control over animations for the agent 
/// </summary>
[RequireComponent(typeof(AgentController))]
[RequireComponent(typeof(SkeletonAnimation))]
public class AgentAnimation : MonoBehaviour
{
    private SkeletonAnimation _animation = null;
    private AgentController _agentController = null;

    //Todo: Add these to a static struct or some such object
    private string _run = "run";
    private string _idle = "idle";
    private string _jump = "jump";
    private string _fall = "falling";
    private string _walk = "walk";
    private string _roll = "roll";
    private string _land = "land";
    private string _crouchIdle = "crouchIdle";
    private string _crouchWalk = "crouchWalk";

    private Spine.Animation _runAnimation;
    private Spine.Animation _idleAnimation;
    private Spine.Animation _jumpAnimation;
    private Spine.Animation _fallAnimation;
    private Spine.Animation _walkAnimation;
    private Spine.Animation _rollAnimation;
    private Spine.Animation _landAnimation;
    private Spine.Animation _crouchIdleAnimation;
    private Spine.Animation _crouchWalkAnimation;
    private Bone _leftShoulder;
    private Bone _rightShoulder;
    private float _crouchAngle = 7.23f;
    private float _crouchOffset = 13f;
    private bool _isMoving;

    public Spine.AnimationState Animation { get { return _animation.state; } }

    private void Awake()
    {
        _agentController = GetComponent<AgentController>();
        _animation = GetComponent<SkeletonAnimation>();
    }

    private void Start()
    {
        _leftShoulder = _animation.skeleton.FindBone("arm_upper_far");
        _rightShoulder = _animation.skeleton.FindBone("arm_upper_near");
        _runAnimation = _animation.skeleton.Data.FindAnimation(_run);
        _idleAnimation = _animation.skeleton.Data.FindAnimation(_idle);
        _jumpAnimation = _animation.skeleton.Data.FindAnimation(_fall);
        _walkAnimation = _animation.skeleton.Data.FindAnimation(_walk);
        _rollAnimation = _animation.skeleton.Data.FindAnimation(_roll);
        _landAnimation = _animation.skeleton.Data.FindAnimation(_land);
        _crouchIdleAnimation = _animation.skeleton.Data.FindAnimation(_crouchIdle);
        _crouchWalkAnimation = _animation.skeleton.Data.FindAnimation(_crouchWalk);
        _fallAnimation = _animation.skeleton.Data.FindAnimation(_fall);
        _animation.UpdateLocal += HandleUpdateLocal;
    }

    void HandleUpdateLocal(ISkeletonAnimation skeletonRenderer)
    {
        if (_agentController.IsCrouched)
            GunAngle(_crouchAngle, _crouchOffset);
        
        else if (_isMoving)
            GunAngle(_crouchAngle, _crouchOffset);  
    }

    private void GunAngle(float angle, float offset)
    {
        _leftShoulder.Rotation = _leftShoulder.Rotation + angle + offset;
        _rightShoulder.Rotation = _rightShoulder.Rotation + angle;       
    }

    private void Move(float x)
    {
        if (_agentController.IsDead)
            return;

        if (InputDeadZone(x))
        {
            _isMoving = false;
            Idle();
            return;
        }

        if (_agentController.IsGrounded)
        {
            _isMoving = true;
            PlayWalkOrRunAnimations(x);
        }
    }

    private static bool InputDeadZone(float x)
    {
        return x > -0.1 && x < 0.1;
    }

    private void PlayWalkOrRunAnimations(float x)
    {
        if ((x > 6 || x < -6) && _animation.state.GetCurrent(0).Animation != _runAnimation)
        {
            _animation.state.SetAnimation(0, _run, true);
        }

        if ((x <= 6 && x >= -6) && _animation.state.GetCurrent(0).Animation != _walkAnimation)
            _animation.state.SetAnimation(0, _walk, true);
    }

    private void Idle()
    {
        if (_animation.state.GetCurrent(0).Animation != _idleAnimation)
        {
            _animation.state.SetAnimation(0, _idle, true);
        }
    }

    private void Jump(float jumpHeight)
    {
        if (_animation.state.GetCurrent(0).Animation != _jumpAnimation)
        {
            _animation.state.SetAnimation(0, _jump, false);
        }
    }

    private void Fall()
    {
        if (_animation.state.GetCurrent(0).Animation != _fallAnimation)
        {
            Debug.Log("Falling");
            _animation.state.SetAnimation(0, _fall, false);
        }
    }

    private void Land()
    {
        if (_animation.state.GetCurrent(0).Animation != _landAnimation)
        {
            Debug.Log("Landing");
            _animation.state.SetAnimation(0, _land, false);
        }
    }

    private void TakeDamage(float amount)
    {
        _animation.state.SetAnimation(0, "hit1", false).Complete += delegate {  };
    }

    private void Death()
    {
        Debug.Log("death Anim");
        _animation.state.SetAnimation(0, "hitBig", false);
    }

    private void Roll(float rollSpeed)
    {
        if (_animation.state.GetCurrent(0).Animation != _rollAnimation)
        {
            Debug.Log("Rolling anim");
            _animation.state.SetAnimation(0, _roll, false).Complete += delegate { _agentController.EndRoll(); };
        }
    }

    private void Crouch(float x)
    {
        if (_agentController.IsDead)
            return;
        
        if (InputDeadZone(x) && _animation.state.GetCurrent(0).Animation != _crouchIdleAnimation)
        {
            _animation.state.SetAnimation(0, _crouchIdle, true);
        }
        else if (!InputDeadZone(x) && _animation.state.GetCurrent(0).Animation != _crouchWalkAnimation)
        {
            _animation.state.SetAnimation(0, _crouchWalk, true);
        }
    }

    private void Update()
    {
        if (IsAbleToMove())
        {
            if (_agentController.IsCrouched)
                Crouch(_agentController.MoveInput);
            else
                Move(_agentController.MoveInput);
        }
    }

    private bool IsAbleToMove()
    {
        return !_agentController.IsRolling 
            && !_agentController.IsDead 
            && !_agentController.IsInAir 
            && !_agentController.IsHit;
    }

    private void OnEnable()
    {
        _agentController.OnJump += Jump;
        _agentController.OnFall += Fall;
        _agentController.OnTakeDamage += TakeDamage;
        _agentController.OnDeath += Death;
        _agentController.OnRoll += Roll;
        _agentController.OnLand += Land;
    }

    private void OnDisable()
    {
        _agentController.OnJump -= Jump;
        _agentController.OnFall -= Fall;
        _agentController.OnTakeDamage -= TakeDamage;
        _agentController.OnDeath -= Death;
        _agentController.OnRoll -= Roll;
        _agentController.OnLand -= Land;
        _animation.UpdateLocal -= HandleUpdateLocal;
    }
}
