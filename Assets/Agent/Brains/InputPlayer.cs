using UnityEngine;

[CreateAssetMenu(menuName = "PlayerBrain")]
public class InputPlayer : Brain
{
    public override void ReadBrain()
    {
        MoveInput = Input.GetAxisRaw(StringReference.Horizontal);
        Roll = Input.GetButtonDown(StringReference.Fire3);
        Attack = Input.GetButtonDown(StringReference.Fire1);
        Jump = Input.GetButtonDown(StringReference.Jump);
        Crouch = Input.GetButton(StringReference.Fire2);
        SwitchWeapons = Input.GetButtonDown(StringReference.WeaponToggle);
        MeleeAttack = Input.GetButton(StringReference.Fire4);
    }
}
