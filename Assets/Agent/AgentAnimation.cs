using Spine.Unity;
using System.Collections;
using UnityEngine;

/// <summary>
/// Provides top layer control over animations for the agent 
/// </summary>
[RequireComponent(typeof(AgentController))]
[RequireComponent(typeof(SkeletonAnimation))]
public class AgentAnimation : MonoBehaviour
{
    private SkeletonAnimation _animation = null;
    private AgentController _agentController = null;

    private string _run = "run";
    private string _idle = "idle";
    private string _jump = "jump";
    private string _fall = "falling";
    private string _reset = "reset";
    private string _walk = "walk";
    private string _roll = "roll";

    private Spine.Animation _runAnimation = null;
    private Spine.Animation _idleAnimation = null;
    private Spine.Animation _jumpAnimation = null;
    private Spine.Animation _fallAnimation = null;
    private Spine.Animation _walkAnimation = null;
    private Spine.Animation _rollAnimation = null;

    public Spine.AnimationState Animation { get { return _animation.state; } }

    private void Awake()
    {
        _agentController = GetComponent<AgentController>();
        _animation = GetComponent<SkeletonAnimation>();

    }

    private void Start()
    {
        _runAnimation = _animation.skeleton.Data.FindAnimation(_run);
        _idleAnimation = _animation.skeleton.Data.FindAnimation(_idle);
        _jumpAnimation = _animation.skeleton.Data.FindAnimation(_fall);
        _walkAnimation = _animation.skeleton.Data.FindAnimation(_walk);
        _rollAnimation = _animation.skeleton.Data.FindAnimation(_roll);
    }

    private void Move(float x)
    {
        if ((x > 6 || x < -6) && _agentController.CurrentState == MovementStateType.Movement && _animation.state.GetCurrent(0).Animation != _runAnimation)
            _animation.state.SetAnimation(0, _run, true);

        if ((x <= 6 && x >= -6) && _agentController.CurrentState == MovementStateType.Movement && _animation.state.GetCurrent(0).Animation != _walkAnimation)
            _animation.state.SetAnimation(0, _walk, true);
    }

    private void Idle()
    {
        if (_agentController.CurrentState == MovementStateType.Idle && _animation.state.GetCurrent(0).Animation != _idleAnimation)
            _animation.state.SetAnimation(0, _idle, true);
    }

    private void Jump(float jumpHeight)
    {
        if (_agentController.CurrentState == MovementStateType.Jumping && _animation.state.GetCurrent(0).Animation != _jumpAnimation)
            _animation.state.SetAnimation(0, _jump, false);
    }

    private void Fall()
    {
        if (_agentController.CurrentState == MovementStateType.Falling && _animation.state.GetCurrent(0).Animation != _fallAnimation)
            _animation.state.SetAnimation(0, _fall, false);
    }

    private void TakeDamage(float amount)
    {
        //TODO: Possibly need to add an after hit system along with counter attack window
        if (_agentController.CurrentState != MovementStateType.Death)
            _animation.state.SetAnimation(0, "hit1", false).Complete += delegate { _agentController.AgentPreviousState(); };
    }

    private void Death()
    {
        _animation.state.SetAnimation(0, "hitBig", false);
    }

    private void Roll(float rollSpeed)
    {
        if (_agentController.CurrentState == MovementStateType.Roll && _animation.state.GetCurrent(0).Animation != _rollAnimation)
        {
            _animation.state.SetAnimation(0, _roll, false).Complete += delegate { _agentController.EndRoll(); Debug.Log("Finished"); };
        }
    }

    private void OnEnable()
    {
        _agentController.OnMove += Move;
        _agentController.OnIdle += Idle;
        _agentController.OnJump += Jump;
        _agentController.OnFall += Fall;
        _agentController.OnTakeDamage += TakeDamage;
        _agentController.OnDeath += Death;
        _agentController.OnRoll += Roll;
    }

    private void OnDisable()
    {
        _agentController.OnMove -= Move;
        _agentController.OnIdle -= Idle;
        _agentController.OnJump -= Jump;
        _agentController.OnFall -= Fall;
        _agentController.OnTakeDamage -= TakeDamage;
        _agentController.OnDeath -= Death;
        _agentController.OnRoll -= Roll;
    }

}
