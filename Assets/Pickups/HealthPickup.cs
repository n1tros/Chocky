using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour, IPickup
{
    [SerializeField] int _healthAmount = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var HealthComponent = collision.GetComponent<Health>();
            HealthComponent.CurrentHealth += _healthAmount;
            gameObject.SetActive(false);
        }
    }
}
