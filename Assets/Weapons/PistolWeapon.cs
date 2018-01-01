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

    private WeaponType _type = WeaponType.Ranged;
    Spine.Bone shoulder;

    public override float BaseDamage { get { return _baseDamage; } }
    public override float CurrentAttackTime { get; set; }
    public override WeaponType Type { get { return _type; } set { _type = value; } }
    public override float Range { get { return _range; } set { _range = value; } }

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

        if (!controller.IsFiring)
        {
            controller.BulletPrefab = _bulletPrefab;
            controller.StartCoroutine(Fire(_fireRate, controller, animator));
        }
    }

    IEnumerator Fire(float delay, WeaponController controller, SkeletonAnimation animator)
    {
        controller.IsFiring = true;
        SpawnBullet(controller);
        animator.state.SetAnimation(1, "pistolNearShoot", false);

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
