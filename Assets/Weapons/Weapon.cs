using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public abstract class Weapon : ScriptableObject
{
    public abstract float BaseDamage { get; }
    public abstract float CurrentAttackTime { get; set; }
    public abstract float Range { get; set; }
    public abstract WeaponType Type { get; set; }

    public abstract void UseWeapon(WeaponController controller, SkeletonAnimation animator);
    public abstract void DrawWeapon(WeaponController controller, SkeletonAnimation animator);
    public abstract void HolsterWeapon(WeaponController controller, SkeletonAnimation animator);
}
