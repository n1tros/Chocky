using Spine.Unity;
using Spine;
using UnityEngine;

public class AgentAnimation
{
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
    private string _takeDamage = "hit1";
    private string _death = "hitBig";

    private Spine.Animation _runAnimation;
    private Spine.Animation _idleAnimation;
    private Spine.Animation _jumpAnimation;
    private Spine.Animation _fallAnimation;
    private Spine.Animation _walkAnimation;
    private Spine.Animation _rollAnimation;
    private Spine.Animation _landAnimation;
    private Spine.Animation _crouchIdleAnimation;
    private Spine.Animation _crouchWalkAnimation;
    private Spine.Animation _takeDamageAnimation;
    private Spine.Animation _deathAnimation;
    private bool _isMoving;

    private readonly SkeletonAnimation _animation;
    private readonly AgentBody _body;
    private readonly Agent _agent;

    public AgentAnimation(Agent agent, SkeletonAnimation animation)
    {
        _agent = agent;
        _animation = animation;
        _body = agent.Body;
        _body.OnJump += Jump;
        _body.OnFall += Fall;
        _body.OnRoll += Roll;
        _body.OnCrouch += Crouch;
        _body.OnTakeDamage += TakeDamage;
        _body.OnDeath += Death;
    }

    public void CacheAgentAnimationOnStart()
    {
        // = _animation.skeleton.FindBone("arm_upper_far");
        //_rightShoulder = _animation.skeleton.FindBone("arm_upper_near");
        _runAnimation = _animation.skeleton.Data.FindAnimation(_run);
        _idleAnimation = _animation.skeleton.Data.FindAnimation(_idle);
        _jumpAnimation = _animation.skeleton.Data.FindAnimation(_fall);
        _walkAnimation = _animation.skeleton.Data.FindAnimation(_walk);
        _rollAnimation = _animation.skeleton.Data.FindAnimation(_roll);
        _landAnimation = _animation.skeleton.Data.FindAnimation(_land);
        _crouchIdleAnimation = _animation.skeleton.Data.FindAnimation(_crouchIdle);
        _crouchWalkAnimation = _animation.skeleton.Data.FindAnimation(_crouchWalk);
        _fallAnimation = _animation.skeleton.Data.FindAnimation(_fall);
        _takeDamageAnimation = _animation.skeleton.Data.FindAnimation(_takeDamage);
        _deathAnimation = _animation.skeleton.Data.FindAnimation(_death);
        //_animation.UpdateLocal += HandleUpdateLocal;
    }

    public void Tick()
    {
        // Check state and play animation in order
        if (_agent.StateMachine.CurrentState is GroundedState || _agent.StateMachine.CurrentState is CrouchState)
        {
            if (_body.MoveInput == 0)
                PlayIdleAnimation();
            else
                PlayWalkOrRunAnimations(_body.MoveInput);
        }
    }

    private void PlayIdleAnimation()
    {
        if (_agent.StateMachine.CurrentState is GroundedState && _animation.state.GetCurrent(0).Animation != _idleAnimation)
        {
            _animation.state.SetAnimation(0, _idle, true);
        }
        else if (_agent.StateMachine.CurrentState is CrouchState && _animation.state.GetCurrent(0).Animation != _crouchIdleAnimation)
        {
            _animation.state.SetAnimation(0, _crouchIdle, true);
        }
    }

    private void PlayWalkOrRunAnimations(float x)
    {
        if (_agent.StateMachine.CurrentState is GroundedState)
        {
            if ((x > 6 || x < -6) && _animation.state.GetCurrent(0).Animation != _runAnimation)
            {
                _animation.state.SetAnimation(0, _run, true);
            }

            if ((x <= 6 && x >= -6) && _animation.state.GetCurrent(0).Animation != _walkAnimation)
                _animation.state.SetAnimation(0, _walk, true);
        }
        else if (_agent.StateMachine.CurrentState is CrouchState && _animation.state.GetCurrent(0).Animation != _crouchWalkAnimation)
        {
            _animation.state.SetAnimation(0, _crouchWalk, true);
        }

    }

    private void Jump()
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
            _animation.state.SetAnimation(0, _fall, false);
        }
    }

    private void Land()
    {
        if (_animation.state.GetCurrent(0).Animation != _landAnimation)
        {
            _animation.state.SetAnimation(0, _land, false);
        }
    }

    private void Roll()
    {
        if (_animation.state.GetCurrent(0).Animation != _rollAnimation)
        {
            _animation.state.SetAnimation(0, _roll, false).Complete += delegate {  };
        }
    }

    private void TakeDamage()
    {
        _animation.state.SetAnimation(0, "hit1", false).Complete += delegate { };
    }

    private void Crouch()
    {
        if (_animation.state.GetCurrent(0).Animation != _crouchIdleAnimation)
        {
            _animation.state.SetAnimation(0, _crouchIdle, true);
        }
    }

    private void Death()
    {
        if (_animation.state.GetCurrent(0).Animation != _deathAnimation)
            _animation.state.SetAnimation(0, _death, false);
    }
}
    /*
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



    private void Idle()
    {
        if (_animation.state.GetCurrent(0).Animation != _idleAnimation)
        {
            _animation.state.SetAnimation(0, _idle, true);
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
*/