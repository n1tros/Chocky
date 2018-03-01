using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private SkeletonAnimation _animator = null;
    
    [SerializeField]
    private Weapon _currentWeapon = null;
    public Weapon Current { get { return _currentWeapon; } set { _currentWeapon = value; } }

    [SerializeField] private Weapon _backupWeapon = null;
    [SerializeField] private BoxCollider2D _swordHitbox = null;
    [SerializeField] private Transform _gunSpawn = null;
    [SerializeField] private GameObject _bulletPrefab = null;

    public bool IsFiring { get; set; }
    public BoxCollider2D MeleeCollider { get { return _swordHitbox; } }
    public GameObject BulletPrefab { get { return _bulletPrefab; } set { _bulletPrefab = value; } }
    public Transform GunSpawn { get { return _gunSpawn; } set { _gunSpawn = value; } }



    public Weapon Backup
    {
        get { return _backupWeapon; }
        set { _backupWeapon = value; }
    }

    public bool WeaponDrawn { get; set; }
    public bool IsReloading { get; internal set; }

    public void ToggleWeapon()
    {
        var weapon = Current;
        Current = Backup;
        Backup = weapon;

        DrawWeapon(true);
    }

    public void UseWeapon()
    {
        Current.UseWeapon(this, _animator);
    }

    public void DrawWeapon(bool weaponOut)
    {
        if (weaponOut)
            Current.DrawWeapon(this, _animator);
        else
            Current.HolsterWeapon(this, _animator);
    }

    public void Reload()
    {
        if (!IsReloading)
            Current.ReloadWeapon(this, _animator);
    }

    private void Awake()
    {
        _animator = GetComponent<SkeletonAnimation>();
    }

    private void Start()
    {
        _animator.state.SetAnimation(1, "reset", true);
        WeaponDrawn = false;
        IsFiring = false;
    }

    private void OnEnable()
    {
        var agent = GetComponent<AgentController>();
        agent.OnAttack += UseWeapon;
        agent.OnDrawWeapon += DrawWeapon;
        agent.OnToggleWeapon += ToggleWeapon;
    }

    private void OnDisable()
    {
        var agent = GetComponent<AgentController>();
        agent.OnAttack -= UseWeapon;
        agent.OnDrawWeapon -= DrawWeapon;
        agent.OnToggleWeapon -= ToggleWeapon;
    }
}
