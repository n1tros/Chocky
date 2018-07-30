using UnityEngine;

[CreateAssetMenu]
public class WeaponDamageBuff : Skill
{
    [SerializeField]
    private float _damageModifier;

    public override void AddSkill(Agent agent)
    {
        var weapons = agent.GetComponent<Weapons>();
   
        weapons.CurrentWeapon.CurrentDamage += weapons.CurrentWeapon.BaseDamage * _damageModifier;
        weapons.ReserveWeapon.CurrentDamage += weapons.ReserveWeapon.BaseDamage * _damageModifier;
    }

    public override void ResetSkill(Agent agent)
    {
        var weapons = agent.GetComponent<Weapons>();

        weapons.CurrentWeapon.CurrentDamage = weapons.CurrentWeapon.BaseDamage;
        weapons.ReserveWeapon.CurrentDamage = weapons.ReserveWeapon.BaseDamage;
    }
}