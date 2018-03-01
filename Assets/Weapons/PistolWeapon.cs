using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Pistol")]
public class PistolWeapon : Weapon
{
    [SerializeField] private float _baseDamage = 1.0f;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private GameObject _bulletPrefab = null;
    [SerializeField] private float _bulletSpeed = 30f;
    [SerializeField] private float _knockbackForce = 10f;
    [SerializeField] private float _range = 50f;

    [SerializeField] private int _baseMaxAmmo = 300;
    [SerializeField] private int _baseMaxClip = 30;
    [SerializeField] private int _currentTotalAmmo = 300;
    [SerializeField] private int _currentAmmoInClip = 30;
    [SerializeField] private int _currentMaxClip = 30;
    [SerializeField] private int _currentMaxAmmo = 300;

    [SerializeField] bool infinateAmmo = false;
    
    private WeaponType _type = WeaponType.Ranged;
    private AmmoType _ammo = AmmoType.Pistol;
    Spine.Bone shoulder;

    public override float BaseDamage { get { return _baseDamage; } }
    public override float CurrentAttackTime { get; set; }
    public override WeaponType Type { get { return _type; } set { _type = value; } }
    public override AmmoType Ammo { get { return _ammo; } set { _ammo = value; } }
    public override float Range { get { return _range; } set { _range = value; } }
    public override int BaseMaxAmmo { get { return _baseMaxAmmo; } set { _baseMaxAmmo = value; } }
    public override int BaseMaxClip { get { return _baseMaxClip; } set { _baseMaxClip = value; } }
    public override int CurrentAmmoInClip { get { return _currentAmmoInClip; } set { _currentAmmoInClip = value; } }
    public override int CurrentMaxClip { get { return _currentMaxClip; } set { _currentMaxClip = value; } }
    public override int CurrentMaxAmmo { get { return _currentMaxAmmo; } set { _currentMaxAmmo = value; } }
    public override int CurrentTotalAmmo
    {
        get { return _currentTotalAmmo; }
        set
        {
            if (value > CurrentMaxAmmo)
                _currentTotalAmmo = CurrentMaxAmmo;
            else
                _currentTotalAmmo = value;
        }
    }

    public override void DrawWeapon(WeaponController controller, SkeletonAnimation animator)
    {
        controller.WeaponDrawn = true;
        animator.state.SetAnimation(1, "pistolNearIdle", true);
    }

    public override void HolsterWeapon(WeaponController controller, SkeletonAnimation animator)
    {
        controller.WeaponDrawn = false;
        animator.state.SetAnimation(1, "reset", true);
    }

    public override void UseWeapon(WeaponController controller, SkeletonAnimation animator)
    {
        controller.WeaponDrawn = true;

        if (!controller.IsFiring && !controller.IsReloading && CurrentAmmoInClip > 0)
        {
            controller.BulletPrefab = _bulletPrefab;
            controller.StartCoroutine(Fire(_fireRate, controller, animator));
        }
        else if (CurrentAmmoInClip == 0 && CurrentTotalAmmo > 0)
        {
            ReloadWeapon(controller, animator);
        }
    }

    public override void ReloadWeapon(WeaponController controller, SkeletonAnimation animator)
    {
        if (!controller.IsReloading)
            controller.StartCoroutine(Reload(controller, animator));
    }

    IEnumerator Reload(WeaponController controller, SkeletonAnimation animator)
    {
        controller.IsReloading = true;
        animator.state.SetAnimation(1, "gunReload2", false).Complete += delegate
        {
            animator.state.SetAnimation(1, "gunIdle", true);
        };

        yield return new WaitForSeconds(1f);

        // TODO: Adjust this to also take in the maximum amount of ammo
        CurrentAmmoInClip = CurrentMaxClip;
        if (! infinateAmmo)
            CurrentTotalAmmo -= CurrentMaxClip;

        controller.IsReloading = false;
    }

    IEnumerator Fire(float delay, WeaponController controller, SkeletonAnimation animator)
    {
        controller.IsFiring = true;
        SpawnBullet(controller);
        animator.state.SetAnimation(1, "pistolNearShoot", false);
        CurrentAmmoInClip--;
        yield return new WaitForSeconds(delay);
        controller.IsFiring = false;
    }

    private void SpawnBullet(WeaponController controller)
    {
        var bulletGameObject = Instantiate(_bulletPrefab, controller.GunSpawn.position, controller.transform.rotation);
        var projectile = bulletGameObject.GetComponent<Projectile>();

        projectile.KnockBackForce = _knockbackForce;
        projectile.Owner = controller.tag;
        projectile.Damage = -BaseDamage;
        var speed = _bulletSpeed;

        if (bulletGameObject.transform.position.x < controller.transform.position.x)
        {
            bulletGameObject.transform.localScale = new Vector3(-bulletGameObject.transform.localScale.x, bulletGameObject.transform.localScale.y, bulletGameObject.transform.localScale.z);
            speed = -_bulletSpeed;
        }

        var rigid = bulletGameObject.GetComponent<Rigidbody2D>();

        rigid.velocity = new Vector2(speed, rigid.velocity.y);
        
        //rigid.AddForce(new Vector2(speed, rigid.velocity.y), ForceMode2D.Impulse);
    }
}
