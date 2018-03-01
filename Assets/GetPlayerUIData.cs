using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPlayerUIData : MonoBehaviour {

    [SerializeField] private AgentController _player;
    [SerializeField] private Text _totalAmmo;
    [SerializeField] private Text _maxClip;
    [SerializeField] private Text _currentClip;

    private WeaponController _weaponController;

    void Start ()
    {
        _weaponController = _player.GetComponent<WeaponController>();
	}
	
	void Update ()
    {
        _totalAmmo.text = _weaponController.Current.CurrentTotalAmmo.ToString();
        _maxClip.text = _weaponController.Current.CurrentMaxClip.ToString();
        _currentClip.text = _weaponController.Current.CurrentAmmoInClip.ToString();
	}
}
