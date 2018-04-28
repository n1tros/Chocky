using Spine.Unity;
using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private SkeletonAnimation _animator;
    private AgentController _agentController;

    [SerializeField]
    private WeaponLoadout _loadout;

    [SerializeField]
    private Weapon _currentWeapon;
    public Weapon Current
    {
        get { return _currentWeapon; }
        set { _currentWeapon = value; }
    }

    [SerializeField]
    private Weapon _weaponSlot1;
    public Weapon Slot1
    {
        get { return _weaponSlot1; }
        set { _weaponSlot1 = value; }
    }

    [SerializeField]
    private Weapon _weaponSlot2;
    public Weapon Slot2
    {
        get { return _weaponSlot2; }
        set { _weaponSlot2 = value; }
    }

    [SerializeField]
    private Weapon _meleeWeapon;
    public Weapon MeleeWeapon
    {
        get { return _meleeWeapon; }
        set { _meleeWeapon = value; }
    }

    //TODO: Move This to Weapon class
    [SerializeField]
    private BoxCollider2D _swordHitbox = null;
    public BoxCollider2D MeleeCollider
    {
        get { return _swordHitbox; }
    }

    //TODO: Move This to Weapon class
    private bool _isFiring;
    public bool IsFiring
    {
        get { return _isFiring; }
        set
        {
            _isFiring = value;
            _agentController.IsAttacking = _isFiring;
        }
    }

    public AudioSource WeaponAudio { get; private set; }
    public bool IsCrouched { get { return _agentController.IsCrouched; } }

    public bool WeaponDrawn { get; set; }
    public bool IsReloading { get; internal set; }

    private ILauncher _launcher;

    private void Awake()
    {
        _animator = GetComponent<SkeletonAnimation>();
        _launcher = GetComponent<ILauncher>();
    }

    private void Start()
    {
        _agentController = GetComponent<AgentController>();
        WeaponAudio = GetComponent<AudioSource>();
        _animator.state.SetAnimation(1, "reset", true);
        WeaponDrawn = false;
        IsFiring = false;
    }

    public void ToggleWeapon(float switchTimeDelay)
    {
        StartCoroutine(SwitchWeapons(switchTimeDelay));
    }

    IEnumerator SwitchWeapons(float switchTimeDelay)
    {
        if (Current.GetType() == Slot1.GetType())
            Current = Slot2;
        else
            Current = Slot1;

        yield return new WaitForSeconds(switchTimeDelay);

        DrawWeapon(true);
    }

    public void UseWeapon()
    {
        if (!IsFiring && !IsReloading)
            Current.UseWeapon(this, _animator, _launcher);
    }

    public void UseMeleeWeapon()
    {
        if (!IsFiring && !IsReloading)
        {
            var weapon = Current;
            Current = MeleeWeapon;
            MeleeWeapon.UseWeapon(this, _animator, _launcher);
            Current = weapon;
        }
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

    private void OnEnable()
    {
        var agent = GetComponent<AgentController>();
        agent.OnAttack += UseWeapon;
        agent.OnDrawWeapon += DrawWeapon;
        agent.OnToggleWeapon += ToggleWeapon;
        agent.OnMeleeAttack += UseMeleeWeapon;
    }

    private void OnDisable()
    {
        var agent = GetComponent<AgentController>();
        agent.OnAttack -= UseWeapon;
        agent.OnDrawWeapon -= DrawWeapon;
        agent.OnToggleWeapon -= ToggleWeapon;
        agent.OnMeleeAttack -= UseMeleeWeapon;
    }
}
