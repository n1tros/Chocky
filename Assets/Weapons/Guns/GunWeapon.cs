using UnityEngine;
using Spine.Unity;
using System;
using System.Collections;

public abstract class GunWeapon : Weapon
{
    [SerializeField]
    private AudioClip _reloadSound;

    [SerializeField]
    protected float _currentAttackTime, _range, _fireRate, _bulletSpeed, _reloadDelay;

    [SerializeField]
    private bool _infinateAmmo;

    [SerializeField]
    protected GameObject _bulletPrefab;

    [SerializeField]
    protected AmmoType _ammoType;

    [SerializeField]
    protected int _baseMaxAmmo, _baseMaxClip, _currentAmmoInClip, _currentTotalAmmo, _currentMaxClip, _currentMaxAmmo;
    public int MaximumClipSize
    {
        get { return _currentMaxClip; }
    }
    public int CurrentAmmoInClip
    {
        get { return _currentAmmoInClip; }
        set { _currentAmmoInClip = value; }
    }

    public int CurrentAmmo
    {
        get { return _currentTotalAmmo; }
        set
        {
            if (value > _currentMaxAmmo)
                _currentTotalAmmo = _currentMaxAmmo;
            else
                _currentTotalAmmo = value;
        }
    }

    public override void UseWeapon(WeaponController controller, SkeletonAnimation animator, ILauncher launcher)
    {
        controller.WeaponDrawn = true;

        if (CurrentAmmoInClip > 0)
        {
            controller.StartCoroutine(Fire(controller, animator, launcher));
        }
        else if (CurrentAmmoInClip == 0 && CurrentAmmo > 0)
        {
            ReloadWeapon(controller, animator);
        }
    }

    IEnumerator Fire(WeaponController controller, SkeletonAnimation animator, ILauncher launcher)
    {
        controller.IsFiring = true;
        PlayWeaponSound(controller);
        SpawnBullet(controller, launcher);
        animator.state.SetAnimation(1, _weaponFireAnimation, false);
        CurrentAmmoInClip--;
        var timeToDelay = CurrentAmmoInClip == 0 ? _reloadDelay : _fireRate;
        yield return new WaitForSeconds(timeToDelay);
        controller.IsFiring = false;
    }

    private void PlayWeaponSound(WeaponController controller)
    {
        if (WeaponSounds.Length > 0)
        {
            controller.WeaponAudio.clip = WeaponSounds[UnityEngine.Random.Range(0, WeaponSounds.Length)];
            controller.WeaponAudio.Play();
        }
    }

    private void SpawnBullet(WeaponController controller, ILauncher launcher)
    {
        launcher.BulletPrefab = _bulletPrefab;
        launcher.Spawn(_bulletSpeed);
    }

    public override void ReloadWeapon(WeaponController controller, SkeletonAnimation animator)
    {
        if (!controller.IsReloading)
            controller.StartCoroutine(Reload(controller, animator));
    }

    private IEnumerator Reload(WeaponController controller, SkeletonAnimation animator)
    {
        controller.IsReloading = true;
        controller.WeaponAudio.clip = _reloadSound;
        controller.WeaponAudio.Play();
        animator.state.SetAnimation(1, _weaponReloadAnimation, false).Complete += delegate
        {
            animator.state.SetAnimation(1, _weaponIdleAnimationName, true);
        };

        yield return new WaitForSeconds(1f);

        // TODO: Adjust this to also take in the maximum amount of ammo
        CurrentAmmoInClip = MaximumClipSize;
        if (!_infinateAmmo)
            CurrentAmmo -= MaximumClipSize;

        controller.IsReloading = false;
    }
}
