using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    private Agent _agent;
    private SkeletonAnimation _animation;
    private ILauncher _launcher;
    private IEnumerator _weaponFire;
    private IEnumerator _weaponReload;
    private IEnumerator _meleeAttack;
    private bool _swappingWeapons;

    [SerializeField]
    private float _weaponSwapSpeed;

    [SerializeField]
    private Weapon _currentWeapon;
    public Weapon CurrentWeapon
    {
        get { return _currentWeapon; }
        set { _currentWeapon = value; }
    }

    [SerializeField]
    private Weapon _reserveWeapon;
    public Weapon ReserveWeapon
    {
        get { return _reserveWeapon; }
        set { _reserveWeapon = value; }
    }

    [SerializeField]
    private MeleeWeapon _meleeWeapon;
    public MeleeWeapon MeleeWeapon
    {
        get { return _meleeWeapon; }
        set { _meleeWeapon = value; }
    }

    [SerializeField]
    private BoxCollider2D _meleeCollider;
    public BoxCollider2D MeleeCollider { get { return _meleeCollider; } }

    private bool _meleeAttacking;

    private void OnEnable()
    {
        _agent = GetComponent<Agent>();
        _animation = GetComponent<SkeletonAnimation>();
        _launcher = GetComponent<ILauncher>();
        _meleeCollider = GetComponentInChildren<MeleeCollider>().GetComponent<BoxCollider2D>();
        _agent.Body.OnAttack += Attack;
        _agent.Body.OnSwitchWeapons += SwitchWeapons;
        _agent.Body.OnMeleeAttack += MeleeAttack;
        _agent.Body.OnDrawMelee += DrawMelee;
        InitialiseCurrentWeapon();
    }

    private void Start()
    {
        MeleeCollider.enabled = false;
    }

    private void OnDrawGizmos()
    {
        Vector2 spherePosition = new Vector2(transform.position.x + 2.5f * transform.localScale.x, transform.position.y + 1.5f);
    }

    private void Attack()
    {
        if (CurrentWeapon.CurrentClip == 0)
        {
            Reload();
        }
        else if (_weaponFire == null && _weaponReload == null)
        {
            _weaponFire = WeaponFire();
            StartCoroutine(_weaponFire);
        }            
    }

    IEnumerator WeaponFire()
    {
        CurrentWeapon.UseWeapon(this, _animation, _launcher);
        yield return new WaitForSeconds(CurrentWeapon.FireRate * _agent.Stats.FireRate);
        _weaponFire = null;
    }

    private void MeleeAttack()
    {
        if (_weaponFire == null && _weaponReload == null && _meleeAttack == null)
        {
            _meleeAttack = MeleeAttacking();
            StartCoroutine(MeleeAttacking());
        }
    }

    IEnumerator MeleeAttacking()
    {
        _meleeAttacking = true;
        _meleeCollider.enabled = true;
        MeleeWeapon.UseWeapon(this, _animation, 1);
        yield return new WaitForSeconds(MeleeWeapon.FireRate * _agent.Stats.MeleeFireRate);
        if (_agent.Brain.MeleeAttack)
        {
            _meleeAttacking = true;

            MeleeWeapon.UseWeapon(this, _animation, 2);
            yield return new WaitForSeconds(MeleeWeapon.FireRate * _agent.Stats.MeleeFireRate);
            _meleeAttacking = false;

        }
        if (_agent.Brain.MeleeAttack)
        {
            _meleeAttacking = true;

            MeleeWeapon.UseWeapon(this, _animation, 3);
            yield return new WaitForSeconds(MeleeWeapon.FireRate * _agent.Stats.MeleeFireRate);
            _meleeAttacking = false;

        }

        if (_agent.Settings.UseMeleeAsMain)
            DrawMelee();
        else
            InitialiseCurrentWeapon();

        _meleeAttacking = false;
        _meleeAttack = null;
    }

    private void SwitchWeapons()
    {
        if (_swappingWeapons == false && _weaponReload == null)
        {
            if (_weaponFire != null)
            {
                StopCoroutine(_weaponFire);
                _weaponFire = null;
            }
            StartCoroutine(SwapWeapons());
        }
    }

    private IEnumerator SwapWeapons()
    {
        _swappingWeapons = true;
        var currenWeapon = CurrentWeapon;
        CurrentWeapon = ReserveWeapon;
        ReserveWeapon = currenWeapon;
        yield return new WaitForSeconds(_weaponSwapSpeed);
        CurrentWeapon.InitiliseWeapon(this, _animation);
        _swappingWeapons = false;
    }

    private void Reload()
    {
        if (_weaponFire == null && _weaponReload == null && _meleeAttack == null)
        {
            _weaponReload = Reloading();
            StartCoroutine(_weaponReload);
        }
    }

    private IEnumerator Reloading()
    {
        CurrentWeapon.ReloadWeapon(this, _animation);
        yield return new WaitForSeconds(CurrentWeapon.ReloadSpeed * _agent.Stats.ReloadSpeed);
        _weaponReload = null;
    }

    public void InitialiseCurrentWeapon()
    {
        CurrentWeapon.InitiliseWeapon(this, _animation);
    }

    public void DrawMelee()
    {
        MeleeWeapon.InitiliseWeapon(this, _animation);
    }

    private void OnDisable()
    {
        _agent.Body.OnAttack -= Attack;
        _agent.Body.OnSwitchWeapons -= SwitchWeapons;
        _agent.Body.OnMeleeAttack -= MeleeAttack;
        _agent.Body.OnDrawMelee -= DrawMelee;
    }
}

