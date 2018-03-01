using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour {

    [SerializeField] int _ammoAmount = 300;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var weaponController = collision.GetComponent<WeaponController>();   
            weaponController.Current.CurrentTotalAmmo += _ammoAmount;
            gameObject.SetActive(false);
        }
    }
}
