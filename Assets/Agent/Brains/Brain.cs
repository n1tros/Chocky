using UnityEngine;

public abstract class Brain : ScriptableObject
{
    private float _moveInput;
    private bool _attack;
    private bool _jump;
    private bool _roll;
    private bool _crouch;
    private bool _switchWeapons;
    private bool _meleeAttack;
    private bool _drawMeleeWeapon;

    public float MoveInput { get { return _moveInput; } set { _moveInput = value; } }
    public bool Attack { get { return _attack; } set { _attack = value; } }
    public bool MeleeAttack { get { return _meleeAttack; } set { _meleeAttack = value; } }

    public bool Jump { get { return _jump; } set { _jump = value; } }
    public bool Roll { get { return _roll; } set { _roll = value; } }
    public bool Crouch { get { return _crouch; } set { _crouch = value; } }
    public bool SwitchWeapons { get { return _switchWeapons; } set { _switchWeapons = value; } }
    public bool DrawMeleeWeapon { get { return _drawMeleeWeapon; } set { _drawMeleeWeapon = value; } }

    public virtual void ReadBrain() { }
    public virtual void SetupBrain(Agent agent) { }
}
