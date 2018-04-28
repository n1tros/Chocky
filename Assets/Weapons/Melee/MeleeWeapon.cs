using Spine.Unity;
using System.Collections;
using UnityEngine;
using Spine;

[CreateAssetMenu(menuName = "Weapon/Sword")]
public class MeleeWeapon : Weapon
{
    [SerializeField] private float _firstSwingDelay = 0.3f;
    [SerializeField] private float _secondSwingDelay = 0.1f;

    [SerializeField] private float _firstSwingSpeed = 1.2f;
    [SerializeField] private float _secondSwingSpeed = 1.5f;

    private bool _firstSwing;

    [SerializeField]
    protected float _currentAttackTime, _range, _fireRate;

    private TrackEntry _firstSwingAnimation;

    public override void UseWeapon(WeaponController controller, SkeletonAnimation animator, ILauncher launcher)
    {
        if (_firstSwing != true)
            Swing(controller, animator);
    }

    private void Swing(WeaponController controller, SkeletonAnimation animator)
    {
        controller.StartCoroutine(FirstSwingDelay(_firstSwingDelay, controller, animator));
    }

    IEnumerator FirstSwingDelay(float delay, WeaponController controller, SkeletonAnimation animator)
    {
        _firstSwing = true;
        yield return new WaitForSeconds(delay);
        var currentAnim = animator.state.SetAnimation(1, "meleeSwing1", false);
        controller.MeleeCollider.enabled = true;
        currentAnim.TimeScale = _firstSwingSpeed;
        currentAnim.Complete += delegate
        {
            controller.MeleeCollider.enabled = false;
            _firstSwing = false;
        };
    }

    private void OnEnable()
    {
        _firstSwing = false;
    }
}
