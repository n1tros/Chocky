using UnityEngine;
[CreateAssetMenu(menuName = "Ability/test")]
public class QuickFireAbility : Ability
{
    public override void AbilityModifier(AgentController agent)
    {
        agent.PlayerStats.FireRate *= 1.2f;
    }
}
