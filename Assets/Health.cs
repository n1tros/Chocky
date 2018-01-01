using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float _currentHealth = 100f;
    //[SerializeField] float _maxHealth = 100f;
    [SerializeField] BoxCollider2D _hotBoxCollider;

    private AgentController _agent = null;

    public float CurrentHealth
    {
        get { return _currentHealth; }
        set
        {
            if (value <= 0)
                Death();
            else
                _currentHealth = value;
        }
    }

    private void Death()
    {
        _hotBoxCollider.enabled = false;
        _agent.AgentDead();

    }

    private void SetHealth(float amount)
    {
        CurrentHealth = CurrentHealth + amount;
        Debug.Log("damage taken " + amount);
    }

    // Use this for initialization
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
