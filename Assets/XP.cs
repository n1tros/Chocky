using System;
using TMPro;
using UnityEngine;

public class XP : MonoBehaviour
{
    [SerializeField] private GameObject _combatText;

    public int TotalXP { get; private set; }
    public event Action<int> OnGainXP = delegate { };

    public void AddXP(int amount)
    {
        TotalXP += amount;
        var combatText = Instantiate(_combatText, transform.position, Quaternion.identity);
        combatText.GetComponent<TextMeshPro>().text = amount.ToString() + " XP";
        combatText.GetComponent<CombatText>().Init(12f, Color.magenta);
        Destroy(combatText, 4f);
        OnGainXP(amount);
    }


}
