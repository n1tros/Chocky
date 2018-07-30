using UnityEngine;
using UnityEngine.UI;

public class DamageDisplay : MonoBehaviour
{

    private Agent _agent;
    [SerializeField] private float _duration = 3f;
    [SerializeField] private Vector2 _startPosition, _endPosition;
    [SerializeField] private GameObject _damage;

    void Awake()
    {
        _agent = GetComponentInParent<Agent>();
    }

    void TakeDamage()
    {

    }

    private void OnEnable()
    {
        _agent.Body.OnTakeDamage += TakeDamage;
    }

    private void OnDisable()
    {
        //_agent.OnTakeDamage -= TakeDamage;
    }
}
