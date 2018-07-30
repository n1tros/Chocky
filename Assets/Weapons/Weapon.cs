using Spine.Unity;
using UnityEngine;

public abstract class Weapon : Item
{
    [SerializeField]
    private float _baseDamage;
    public float BaseDamage { get { return _baseDamage; } }

    public float CurrentDamage { get; set; }

    [SerializeField]
    private GameObject _bulletPrefab;

    [SerializeField]
    private float _projectileVelocity;
    public float ProjectileVelocity { get { return _projectileVelocity; } }

    [SerializeField]
    private float _fireRate;
    public float FireRate { get { return _fireRate; } }

    [SerializeField]
    private float _weaponForce;
    public float WeaponForce { get { return _weaponForce; } }

    [SerializeField]
    private float _reloadSpeed;
    public float ReloadSpeed { get { return _reloadSpeed; } }

    [SerializeField]
    private int _baseMaxClip;
    public int BaseMaxClip { get { return _baseMaxClip; } set { _baseMaxClip = value; } }

    public int CurrentMaxClip { get; set; }
    public int CurrentClip { get; set; }
    public bool MeleeAttacking { get; set; }

    [SerializeField]
    private int _baseMaxAmmo;
    public int BaseMaxAmmo { get { return _baseMaxAmmo; } set { _baseMaxAmmo = value; } }
    public int CurrentAmmo { get; set; }

    [SerializeField]
    private AudioClip _shootSound;
    public AudioClip ShootSound { get { return _shootSound; } }

    [SerializeField]
    private AudioClip _reloadSound;
    public AudioClip ReloadSound { get { return _reloadSound; } }

    protected string _weaponIdleAnimationName, _weaponReloadAnimation, _weaponFireAnimation;

    public virtual void InitiliseWeapon(Weapons weapons, SkeletonAnimation animation)
    {
        animation.state.SetAnimation(1, _weaponIdleAnimationName, false);
    }

    public virtual void UseWeapon(Weapons weapons, SkeletonAnimation animation, ILauncher launcher)
    {
        animation.state.SetAnimation(1, _weaponFireAnimation, false);
        launcher.AttackSpawn(_bulletPrefab, this);
        PlaySound(weapons, ShootSound);
        CurrentClip--;
    }

    private void PlaySound(Weapons weapons, AudioClip sound)
    {
        var audio = weapons.GetComponent<AudioSource>();
        if (sound)
        {
            audio.clip = sound;
            audio.Play();
        }
    }

    public virtual void ReloadWeapon(Weapons weapons, SkeletonAnimation animation)
    {
        if (CurrentAmmo > 0)
        {
            if (_weaponReloadAnimation != "")
                animation.state.SetAnimation(1, _weaponReloadAnimation, false);

            var maxClip = Mathf.Clamp(CurrentAmmo, 1, CurrentMaxClip);
            CurrentAmmo -= maxClip;
            CurrentClip = maxClip;
            PlaySound(weapons, ReloadSound);
        }
    }

    private void OnEnable()
    {
        CurrentMaxClip = _baseMaxClip;
        CurrentAmmo = _baseMaxAmmo;
    }
}
//    [SerializeField]
//    private AudioClip[] _weaponSounds;
//    public AudioClip[] WeaponSounds { get { return _weaponSounds; }}

//    [SerializeField]
//    private float _knockBackForce;
//    public float KnockbackForce { get { return _knockBackForce; }}

//    [SerializeField]
//    private WeaponType _weaponType;

//    [SerializeField]
//    internal bool IsMelee;

//    public WeaponType Type
//    {
//        get { return _weaponType; }
//    }

//    public virtual void UseWeapon(WeaponController controller, SkeletonAnimation animation, ILauncher launcher) { }
//    public virtual void DrawWeapon(WeaponController controller, SkeletonAnimation animator)
//    {
//        controller.WeaponDrawn = true;
//        animator.state.SetAnimation(1, _weaponIdleAnimationName, true);
//    }
//    public virtual void HolsterWeapon(WeaponController controller, SkeletonAnimation animator)
//    {
//        controller.WeaponDrawn = false;
//        animator.state.SetAnimation(1, "reset", true);
//    }
//    public virtual void ReloadWeapon(WeaponController controller, SkeletonAnimation animator) { }
//}
