using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Shotgun")]
public class Shotgun : Weapon
{
    private void OnEnable()
    {
        _weaponIdleAnimationName = "gunIdle";
        _weaponReloadAnimation = "gunReload1";
        _weaponFireAnimation = "gunShoot";
        CurrentDamage = BaseDamage;
        CurrentMaxClip = BaseMaxClip;
        CurrentAmmo = BaseMaxAmmo;
    }
}