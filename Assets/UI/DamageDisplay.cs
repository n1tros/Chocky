using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageDisplay : MonoBehaviour {

    private AgentController _agent;
    [SerializeField] private float _duration = 3f;
    [SerializeField] private Vector2 _startPosition, _endPosition;
    [SerializeField] private Text _damage;

    void Awake()
    {
        _agent = GetComponentInParent<AgentController>();
    }

    void TakeDamage(float amount)
    {
        var damageObject = Instantiate(_damage, transform);
        damageObject.text = amount.ToString();
        damageObject.rectTransform.anchoredPosition = _startPosition;
        Destroy(damageObject, _duration);
    }

    private void OnEnable()
    {
        _agent.OnTakeDamage += TakeDamage;
    }

    private void OnDisable()
    {
        _agent.OnTakeDamage -= TakeDamage;
    }
}
