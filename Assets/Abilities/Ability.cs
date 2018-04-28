using UnityEngine;
public enum AbilityType
{
    Default,
    CloseCombat,
    RapidFire,
    QuickReload
}

public abstract class Ability : ScriptableObject
{
    [SerializeField]
    private AbilityType _name;
    public AbilityType Name { get { return _name; } }
    public bool Active { get; set; }

    public virtual void AbilityModifier(AgentController agent)
    {

    }
}
