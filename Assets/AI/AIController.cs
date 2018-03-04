using FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentController))]
public class AIController : MonoBehaviour
{
    [SerializeField] private Transform _leftEdge = null;
    [SerializeField] private Transform _rightEdge = null;
    [SerializeField] private AgentController _target = null;
    [SerializeField] private CircleCollider2D MeleeRangeCollider = null;

    [SerializeField]
    private Brain _brain = null;
    public Brain CurrentBrain
    {
        get { return _brain; }
        set { _brain = value; }
    }

    private StateMachine _stateMachine = null;
    public StateMachine StateMachine { get { return _stateMachine; } }

    private AgentController _agent = null;

    public bool TargetInMeleeRange { get; set; }
    public bool TargetInGunRange { get; set; }

    public AgentController Target
    {
        get { return _target; }
        set { _target = value; }
    }
    public AgentController PreviousTarget { get; set; }

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
        _stateMachine = new StateMachine(Agent);
        Debug.Log(_stateMachine);
        TargetInMeleeRange = false;
        TargetInGunRange = false;
        _stateMachine.ChangeState(new AIStartState(_agent));
    }

    private void Update()
    {
        _stateMachine.CurrentState.Tick();
    }

    private void FixedUpdate()
    {
        _stateMachine.CurrentState.FixedTick();
    }

    public void Patrol()
    {
        _stateMachine.ChangeState(new AIPatrolState(_agent));
    }

    public void Idle()
    {
        _stateMachine.ChangeState(new AIIdleState(_agent));
    }

    public void Search()
    {
        _stateMachine.ChangeState(new AISearchState(_agent));
    }

    public void TargetAgent(AgentController agent)
    {
        Target = agent;

        var weapon = GetComponent<WeaponController>();
        if(weapon.Current.Type == WeaponType.Melee)
        {
            MeleeRangeCollider.enabled = true;
            _stateMachine.ChangeState(new AIMeleeAttackState(_agent));
        }
        else if(weapon.Current.Type == WeaponType.Ranged)
        {
            _stateMachine.ChangeState(new AIRangedAttackState(_agent));
        }

    }

    public void TargetLoss()
    {
        if (Target != null)
        {
            PreviousTarget = Target;
            Target = null;

            _stateMachine.ChangeState(new AISearchState(_agent));
        }
    }

    public void Death()
    {
        _stateMachine.ChangeState(new AIDeathState(_agent));
    }

    public void TakeDamage()
    {
        _stateMachine.ChangeState(new AIDamageState(_agent));
    }

    public void Attack()
    {

    }

}
