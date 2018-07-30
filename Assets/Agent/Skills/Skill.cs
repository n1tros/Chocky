using UnityEngine;

public abstract class Skill : ScriptableObject
{
    [SerializeField] private int _xpToUnlock;
    public int XPToUnlock
    {
        get { return _xpToUnlock; }
    }

    public bool Unlocked { get; set; }

    [SerializeField] private bool _selected;
    public bool Selected
    {
        get { return _selected; }
        set { _selected = value; }
    }

    [SerializeField] private Sprite _icon;
    public Sprite Icon
    {
        get { return _icon; }
    }


    public virtual void AddSkill(Agent agent) { }
    public virtual void ResetSkill(Agent agent) { }
}
