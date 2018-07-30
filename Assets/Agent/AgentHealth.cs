using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Agent))]
public class AgentHealth : MonoBehaviour, IDamagable
{
    [SerializeField] float _currentHealth = 100f;
    [SerializeField] float _maxHealth = 100f;
    [SerializeField] BoxCollider2D _hotBoxCollider;
    [SerializeField] PlayerHealthBar _healthBarObject;
    [SerializeField] private GameObject _damage;
    [SerializeField] private float _duration = 3f;
    [SerializeField] private Vector2 _startPosition, _endPosition;
    [SerializeField] private AudioClip _hitSound, _deathSound;

    private Agent _agent;

    public float CurrentHealth
    {
        get { return _currentHealth; }
        set
        {
            if (value <= 0)
            {
                _currentHealth = 0;
                Death();
            }
            else if (value >= _maxHealth)
                _currentHealth = _maxHealth;
            else
                _currentHealth = value;

            UpdateHealthBar();
        }
    }

    public float HealthAsPercentage { get { return _currentHealth / _maxHealth; } }
    public bool IsDead { get; set; }
    public bool IsInvulnerable { get; set; }
    
    private void Awake()
    {
        _agent = GetComponent<Agent>();
    }

    // TODO: Maybe have a bool and the changestate can happen from withing states.
    public void TakeDamage(float damageAmount, float forceAmount, Vector2 forceDirection)
    {
        if (!IsDead || !IsInvulnerable)
        {
            StartCoroutine(InvulnerableTimer(damageAmount, forceAmount, forceDirection));
            SpawnDamageAmountText(damageAmount);
            //TODO : Sort out Agent Audio
            var audio = GetComponent<AudioSource>();
            audio.clip = _hitSound;
            audio.Play();
        }
    }

    IEnumerator InvulnerableTimer(float amount, float force, Vector2 direction)
    {
        IsInvulnerable = true;
        _agent.StateMachine.ChangeState(new DamagedState(_agent));
        _agent.Physics.Impact(force, direction);
        CurrentHealth -= amount;
        yield return new WaitForSeconds(_agent.Settings.InvulnerabilityTimeWhenHit);
        IsInvulnerable = false;
    }

    private void SpawnDamageAmountText(float amount)
    {
        var damageObject = Instantiate(_damage, transform.position, Quaternion.identity);
        damageObject.GetComponent<TextMeshPro>().text = amount.ToString();
        damageObject.GetComponent<CombatText>().Init(6f, Color.red);
        Destroy(damageObject, _duration);
    }

    public void UpdateHealthBar()
    {
        _healthBarObject.HealthUpdate(HealthAsPercentage);
    }

    private void Death()
    {
        var audio = GetComponent<AudioSource>();
        audio.clip = _deathSound;
        audio.Play();
        _agent.StopAllCoroutines();
        _hotBoxCollider.enabled = false;
        IsDead = true;
        //TODO : Sort out Agent Audio
        _agent.Body.Dead();
    }
}
