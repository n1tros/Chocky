using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPlayerUIData : MonoBehaviour {

    [SerializeField] private AgentController _player;
    [SerializeField] private Text _totalAmmo;
    [SerializeField] private Text _maxClip;
    [SerializeField] private Text _currentClip;
    [SerializeField] private Image _weaponSprite;

    void Start ()
    {
        ChangeImage(0);
    }

    void Update ()
    {
        /*
        _totalAmmo.text = _weaponController.Current.CurrentAmmo.ToString();
        _maxClip.text = _weaponController.Current.MaximumClipSize.ToString();
        _currentClip.text = _weaponController.Current.CurrentAmmoInClip.ToString();
        */
	}

    void ChangeImage(float z)
    {
        /*
        _weaponSprite.sprite = _weaponController.Current.Icon;
        */
    }

    private void OnEnable()
    {
        _player.OnToggleWeapon += ChangeImage;
    }

}
