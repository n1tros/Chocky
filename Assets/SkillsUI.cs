using System.Collections;
using UnityEngine;

public class SkillsUI : MonoBehaviour
{
    [SerializeField] private GetPlayerUIData _uiData;

    [SerializeField]
    private SkillSlotUI[] _skillSlots;

    private void Start()
    {
        _uiData.Player.GetComponent<AgentSkills>().OnUnlockSKill += UpdateSkills;

        PlaceSkillsInSlots();
    }

    private void PlaceSkillsInSlots()
    {
        _skillSlots = GetComponentsInChildren<SkillSlotUI>();
        var skills = _uiData.Player.GetComponent<AgentSkills>().SkillTree;
        for (int i = 0; i < _skillSlots.Length; i++)
        {
            if (skills[i] != null)
                _skillSlots[i].UpdateSkillSlot(skills[i]);
        }
    }

    public void UpdateSkills()
    {
        for (int i = 0; i < _skillSlots.Length; i++)
        {
            if (_skillSlots[i] != null)
                _skillSlots[i].RefreshUI();
        }
    }
    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        _uiData.Player.GetComponent<AgentSkills>().OnUnlockSKill -= UpdateSkills;
    }
}
