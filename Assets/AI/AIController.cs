using FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentController))]
public class AIController : MonoBehaviour
{
    //TODO: Serialise private fields make properties
    [SerializeField] private Transform _leftEdge = null;
    [SerializeField] private Transform _rightEdge = null;
    [SerializeField] private AgentController _target = null;
    [SerializeField] private CircleCollider2D MeleeRangeCollider = null;
    [SerializeField] private Brain _brain = null;

    private AIStateMachine _stateMachine = null;
    private AgentController _agent = null;

    public AIStateType CurrentState { get { return _stateMachine.CurrentState; } }
    public AIStateType PreviousState { get { return _stateMachine.PreviousState; } }

    public bool TargetInMeleeRange { get; set; }
    public bool TargetInGunRange { get; set; }

    public AgentController Target
    {
        get { return _target; }
        set { _target = value; }
    }
    public AgentController PreviousTarget { get; set; }
    public Brain CurrentBrain
    {
        get { return _brain; }
        set { _brain = value; }
    }
    public Transform LeftEdge { get { return _leftEdge; } }
    public Transform RightEdge { get { return _rightEdge; } }
    public AgentController Agent
    {
        get { return _agent; }
        set { _agent = value; }
    }
    public AIStateType AttackType { get; set; }

    private void Start()
    {
        Agent = GetComponent<AgentController>();
        _stateMachine = new AIStateMachine(Agent);
        _stateMachine.ChangeState(AIStateType.AIStart);
        TargetInMeleeRange = false;
        TargetInGunRange = false;
    }

    private void Update()
    {
        _brain.DecideState(this);
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }

    public void Patrol()
    {
        _stateMachine.ChangeState(AIStateType.AIPatrol);
    }

    public void Idle()
    {
        _stateMachine.ChangeState(AIStateType.AIIdle);
        _agent.AgentIdle();
    }

    public void TargetAgent(AgentController agent)
    {
        Target = agent;

        var weapon = GetComponent<WeaponController>();
        if(weapon.Current.Type == WeaponType.Melee)
        {
            MeleeRangeCollider.enabled = true;
            _stateMachine.ChangeState(AIStateType.AIMeleeAttack);
        }
        else if(weapon.Current.Type == WeaponType.Ranged)
        {
            _stateMachine.ChangeState(AIStateType.AIRangedAttack);
        }

    }

    public void TargetLoss()
    {
        if (Target != null)
        {
            PreviousTarget = Target;
            Target = null;

            _stateMachine.ChangeState(AIStateType.AISearch);
        }
    }

    public void Death()
    {
        _stateMachine.ChangeState(AIStateType.AIDeath);
    }

    public void TakeDamage()
    {
        _stateMachine.ChangeState(AIStateType.AIDamage);
    }

    public void Attack()
    {

    }
}
