using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilityMap")]
public class AbilityMap : ScriptableObject
{
    [Header("Abilities")]
    [SerializeField]
    List<Ability> _baseAbilities;

    public List<Ability> CurrentAbilities
    {
        get
        {
            return _baseAbilities.Where(p => p.Active == true).ToList();
        }
    }

}
