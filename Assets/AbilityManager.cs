using UnityEngine;
public enum Characters
{
    ShotgunTed,
    PistolPete,
    AlTheAssassin
}
public class AbilityManager : MonoBehaviour
{
    /* Ability manager needs to get information about when XP is earned and store this
     * It can track which abilities are unlocked by each Character
     * holds available abilitys for each character */
    public int TotalXP { get; set; }

    private void ProcessXP(int amount)
    {
        TotalXP += amount;
    }
}