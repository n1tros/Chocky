using Spine.Unity;
using UnityEngine;

public abstract class Weapon : Item
{
    [SerializeField]
    private AudioClip[] _weaponSounds;
    public AudioClip[] WeaponSounds { get { return _weaponSounds; }}

    [SerializeField]
    private float _knockBackForce;
    public float KnockbackForce { get { return _knockBackForce; }}

    [SerializeField]
    private float _baseDamage;
    public float BaseDamage { get { return _baseDamage; }}

    [SerializeField]
    protected string _weaponIdleAnimationName, _weaponReloadAnimation, _weaponFireAnimation;

    [SerializeField]
    private WeaponType _weaponType;
    internal bool IsMelee;

    public WeaponType Type
    {
        get { return _weaponType; }
    }

    public virtual void UseWeapon(WeaponController controller, SkeletonAnimation animation, ILauncher launcher) { }
    public virtual void DrawWeapon(WeaponController controller, SkeletonAnimation animator)
    {
        controller.WeaponDrawn = true;
        animator.state.SetAnimation(1, _weaponIdleAnimationName, true);
    }
    public virtual void HolsterWeapon(WeaponController controller, SkeletonAnimation animator)
    {
        controller.WeaponDrawn = false;
        animator.state.SetAnimation(1, "reset", true);
    }
    public virtual void ReloadWeapon(WeaponController controller, SkeletonAnimation animator) { }
}
