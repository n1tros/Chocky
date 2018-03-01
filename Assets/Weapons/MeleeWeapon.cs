using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Sword")]
public class MeleeWeapon : Weapon
{
    [SerializeField] private float _baseDamage = 1.0f;
    [SerializeField] private float _firstSwingDelay = 0.3f;
    [SerializeField] private float _secondSwingDelay = 0.1f;

    [SerializeField] private float _firstSwingSpeed = 1.2f;
    [SerializeField] private float _secondSwingSpeed = 1.5f;

    private WeaponType _type = WeaponType.Melee;
    private AmmoType _ammo = AmmoType.Melee;

    private bool _firstSwing = false, _secondSwing = false;

    public override float BaseDamage { get { return _baseDamage; } }
    public override float Range { get; set; }
    public override float CurrentAttackTime { get; set; }
    public override WeaponType Type { get { return _type; } set { _type = value; } }
    public override AmmoType Ammo { get { return _ammo; } set { _ammo = value; } }
    public override int BaseMaxAmmo { get; set; }
    public override int BaseMaxClip { get; set; }
    public override int CurrentTotalAmmo { get; set; }
    public override int CurrentAmmoInClip { get; set; }
    public override int CurrentMaxClip { get; set; }
    public override int CurrentMaxAmmo { get; set; }


    public override void DrawWeapon(WeaponController controller, SkeletonAnimation animator)
    {
        controller.WeaponDrawn = true;
        animator.state.SetAnimation(1, "meleeIdle", true);
    }

    public override void HolsterWeapon(WeaponController controller, SkeletonAnimation animator)
    {
        controller.WeaponDrawn = false;
        animator.state.SetAnimation(1, "reset", true);
    }

    public override void UseWeapon(WeaponController controller, SkeletonAnimation animator)
    {
        controller.WeaponDrawn = true;
        if (_firstSwing == true && _secondSwing == false)
        {
            SecondSwing(controller, animator);
            return;
        }
        else if (_firstSwing == true || _secondSwing == true)
            return;

        FirstSwing(controller, animator);
    }

    private void FirstSwing(WeaponController controller, SkeletonAnimation animator)
    {
        CurrentAttackTime = _firstSwingDelay;
        controller.MeleeCollider.enabled = true;
        controller.StartCoroutine(FirstSwingDelay(_firstSwingDelay, controller));
        animator.state.SetAnimation(1, "meleeSwing1", false).TimeScale = _firstSwingSpeed;
    }

    IEnumerator FirstSwingDelay(float delay, WeaponController controller)
    {
        _firstSwing = true;
        yield return new WaitForSeconds(delay);
        _firstSwing = false;
        controller.MeleeCollider.enabled = false;
    }

    private void SecondSwing(WeaponController controller, SkeletonAnimation animator)
    {
        _secondSwing = true;
        var x = animator.state.AddAnimation(1, "meleeSwing2", false, 0);
        x.TimeScale = _secondSwingSpeed;
        x.Start += delegate 
        {
            CurrentAttackTime = _secondSwingDelay;
            controller.MeleeCollider.enabled = true;
            controller.StartCoroutine(SecondSwingDelay(_secondSwingDelay, controller));
        };
    }

    IEnumerator SecondSwingDelay(float delay, WeaponController controller)
    {
        //TODO: What happens if user changes weapon mid way through?
        yield return new WaitForSeconds(delay);
        _secondSwing = false;
        controller.MeleeCollider.enabled = false;
    }

    private void OnEnable()
    {
        _firstSwing = false;
        _secondSwing = false;
    }

    public override void ReloadWeapon(WeaponController controller, SkeletonAnimation animator)
    {
        Debug.Log("Cannot reload a Sword!.......... Yet");
    }
}
