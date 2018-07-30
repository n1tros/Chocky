using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Pistol")]
public class PistolWeapon : Weapon
{
    private void OnEnable()
    {
        _weaponIdleAnimationName = "pistolFarIdle";
        _weaponReloadAnimation = "";
        _weaponFireAnimation = "pistolFarShoot";
        CurrentDamage = BaseDamage;
        CurrentDamage = BaseDamage;
        CurrentMaxClip = BaseMaxClip;
        CurrentAmmo = BaseMaxAmmo;
    }
}
