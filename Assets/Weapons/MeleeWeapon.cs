using Spine.Unity;
using UnityEngine;

[CreateAssetMenu]
public class MeleeWeapon : ScriptableObject
{
    [SerializeField]
    private LayerMask _hitLayer;

    [SerializeField]
    ContactFilter2D contactFilter;

    [SerializeField]
    Collider2D colliders;


    [SerializeField]
    private float _baseDamage;
    public float BaseDamage { get { return _baseDamage; } }

    public float CurrentDamage { get; set; }

    [SerializeField]
    private float _fireRate;
    public float FireRate { get { return _fireRate; } }

    [SerializeField]
    private float _weaponForce;
    public float WeaponForce { get { return _weaponForce; } }

    protected readonly string _weaponIdleAnimationName = "meleeIdle";
    protected readonly string _weaponSwing1 = "meleeSwing1-fullBody";
    protected readonly string _weaponSwing2 = "meleeSwing2-fullBody";
    protected readonly string _weaponSwing3 = "meleeSwing3-fullBody";

    private void OnEnable()
    {
        CurrentDamage = BaseDamage;
    }

    public virtual void InitiliseWeapon(Weapons weapons, SkeletonAnimation animation)
    {
        animation.state.SetAnimation(1, _weaponIdleAnimationName, false);
    }

    public virtual void UseWeapon(Weapons weapons, SkeletonAnimation animation, int animationNumber)
    {
        weapons.MeleeCollider.enabled = true;
        switch (animationNumber)
        {
            case 1:
                animation.state.SetAnimation(1, _weaponSwing1, false).Complete += delegate
                {
                    weapons.MeleeCollider.enabled = false;
                };
                break;
            case 2:
                animation.state.SetAnimation(1, _weaponSwing2, false).Complete += delegate
                {
                    weapons.MeleeCollider.enabled = false;
                };
                break;
            case 3:
                animation.state.SetAnimation(1, _weaponSwing3, false).Complete += delegate
                {
                    weapons.MeleeCollider.enabled = false;
                };
                break;

            default:
                break;
        }
        /*
        Vector2 spherePosition = new Vector2(weapons.transform.position.x + 2.5f * weapons.transform.localScale.x, weapons.transform.position.y + 1.5f);
        Collider2D x = Physics2D.OverlapCircle(spherePosition, 2f, _hitLayer);

        if (x != null && (x.transform.parent.gameObject != weapons.gameObject))
        {
            Vector2 knockBackDir = (x.transform.position - weapons.transform.position).normalized;
            Debug.Log(x.name);
            Debug.Log(x.GetComponentInParent<IDamagable>());
            x.GetComponentInParent<IDamagable>().TakeDamage(CurrentDamage, WeaponForce, knockBackDir);
        }
        if (animationNumber == 1)
        {
            animation.state.SetAnimation(1, _weaponSwing1, false).Complete += delegate
            {
                //ContactFilter2D contactFilter;

            };
        }
        else if (animationNumber == 2)
        {
            animation.state.SetAnimation(1, _weaponSwing2, false).Complete += delegate
            {
                
            };
        }

        else if (animationNumber == 3)
        {
            animation.state.SetAnimation(1, _weaponSwing3, false);
        }
        */


        // IF UseMelee still true play next animation 
    }
}
