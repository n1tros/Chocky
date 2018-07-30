using UnityEngine;
using FSM;
using Spine.Unity;
using System.Collections.Generic;

public class Agent : MonoBehaviour
{
    [SerializeField] private AgentSettings _agentSettings;
    [SerializeField] private Brain _brain;

    private AgentBody _body;
    private AgentPhysics _physics;
    private AgentAnimation _animation;
    private StateMachine _state;
    private GroundCheck _groundcheck;
    private AgentSkills _agentSkills;
    private AgentStats _stats;
    private AgentHealth _health;

    public AgentBody Body { get { return _body; }}
    public AgentPhysics Physics { get { return _physics; }}
    public AgentAnimation Animation { get { return _animation; }}
    public StateMachine StateMachine { get { return _state; }}
    public AgentSettings Settings { get { return _agentSettings; } }
    public AgentStats Stats { get { return _stats; } }
    public GroundCheck GroundCheck { get { return _groundcheck; } }
    public Brain Brain { get { return _brain; } }
    public AgentHealth Health { get { return _health; } }

    private void Awake()
    {
        _body = new AgentBody();
        _physics = new AgentPhysics(transform, GetComponent<Rigidbody2D>(), this);
        _state = new StateMachine();
        _animation = new AgentAnimation(this, GetComponent<SkeletonAnimation>());
        _groundcheck = GetComponent<GroundCheck>();
        _stats = new AgentStats(this);
        _agentSkills = GetComponent<AgentSkills>();
        _health = GetComponent<AgentHealth>();
        _state.ChangeState(new GroundedState(this));
        
        if (tag == "Player")
            DeathManager.Player = this;
        else
        {
            if (DeathManager.Enemies == null)
                DeathManager.Enemies = new List<Agent>();
            DeathManager.Enemies.Add(this);
        }
    }

    private void Start()
    {
        _animation.CacheAgentAnimationOnStart();
        if (Settings.UseAI)
            _brain.SetupBrain(this);
        
        if (_agentSkills != null)
            _agentSkills.ApplyAllSkills();
         
    }

    private void Update()
    {
        _brain.ReadBrain();
        _state.CurrentState.Tick();
        _animation.Tick();
    }

    private void FixedUpdate()
    {
        _physics.FixedTick();
    }
}
