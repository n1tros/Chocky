using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float _currentHealth = 100f;
    [SerializeField] float _maxHealth = 100f;
    [SerializeField] BoxCollider2D _hotBoxCollider;
    [SerializeField] PlayerHealthBar _healthBarObject;

    private AgentController _agent = null;

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

    public float HealthAsPercentage
    {
        get
        {
            return _currentHealth / _maxHealth;
        }
    }

    public void UpdateHealthBar()
    {
        _healthBarObject.HealthUpdate(HealthAsPercentage);
    }

    private void Death()
    {
        _hotBoxCollider.enabled = false;
        _agent.Dead();
    }

    private void SetHealth(float amount)
    {
        CurrentHealth = CurrentHealth + amount;
    }

    public void TakeDamage(float amount)
    {
        if (!_agent.IsDead || !_agent.IsInvulnerable)
            StartCoroutine(InvulnerableTimer(amount));
    }

    IEnumerator InvulnerableTimer(float amount)
    {
        _agent.TakeDamage(amount);
        _agent.IsInvulnerable = true;
        yield return new WaitForSeconds(_agent.AgentSettings.InvulnerabilityTime);
        _agent.IsInvulnerable = false;
    }
    
    void Awake ()
    {
        _agent = GetComponent<AgentController>();
	}

    private void OnEnable()
    {
        _agent.OnTakeDamage += SetHealth;
    }

    private void OnDisable()
    {
        _agent.OnTakeDamage -= SetHealth;
    }
}
