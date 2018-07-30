//using FSM;
//using UnityEngine;

//[RequireComponent(typeof(AgentAction))]
//public class AIController : MonoBehaviour
//{
//    [SerializeField] private Transform _leftEdge = null;
//    [SerializeField] private Transform _rightEdge = null;
//    [SerializeField] private AgentAction _target = null;
//    [SerializeField] private CircleCollider2D MeleeRangeCollider = null;

//    [SerializeField]
//    private Brain _brain = null;
//    public Brain CurrentBrain
//    {
//        get { return _brain; }
//        set { _brain = value; }
//    }

//    [SerializeField]
//    public bool _isMelee;

//    public bool IsAttacking { get; set; }
//    public bool InAttackState { get; set; }

//    private StateMachine _stateMachine = null;
//    public StateMachine StateMachine { get { return _stateMachine; } }

//    private AgentAction _agent;
    
//    public bool TargetInMeleeRange { get; set; }
//    public bool TargetInGunRange { get; set; }

//    public AgentAction Target
//    {
//        get { return _target; }
//        set { _target = value; }
//    }
//    public AgentAction PreviousTarget { get; set; }

//    public Transform LeftEdge { get { return _leftEdge; } }
//    public Transform RightEdge { get { return _rightEdge; } }
//    public AgentAction Agent
//    {
//        get { return _agent; }
//        set { _agent = value; }
//    }
//    public AIStateType AttackType { get; set; }

//    private void Start()
//    {
//        Agent = GetComponent<AgentAction>();
//        _stateMachine = new StateMachine(Agent);
//        _stateMachine.ChangeState(new AIStartState(_agent));
//    }

//    private void Update()
//    {
//        _stateMachine.CurrentState.Tick();
//       // Debug.Log(_stateMachine.CurrentState.GetType().ToString());
//    }

//    private void FixedUpdate()
//    {
//        _stateMachine.CurrentState.FixedTick();
//    }

//    public void TargetAgent(AgentAction agent)
//    {
//        Target = agent;
//    }

//    public void TargetLoss()
//    {
//        if (Target != null)
//        {
//            PreviousTarget = Target;
//            Target = null;

//            _stateMachine.ChangeState(new AISearchState(_agent));
//        }
//    }

//    public void ChangeState(AIState state)
//    {
//        if (_stateMachine.CurrentState != state)
//            _stateMachine.ChangeState(state);
//    }

//    public void Death()
//    {
//        if(_stateMachine.CurrentState.GetType() != typeof(AIDeathState))
//            _stateMachine.ChangeState(new AIDeathState(_agent));
//    }

//    public void TakeDamage()
//    {
//        _stateMachine.ChangeState(new AIDamageState(_agent));
//    }
//}
