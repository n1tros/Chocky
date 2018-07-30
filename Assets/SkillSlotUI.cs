using UnityEngine;
using UnityEngine.UI;

public class SkillSlotUI : MonoBehaviour
{
    private Skill _skillInSlot;

    [SerializeField] private Image _image;

    public bool IsActive { get; set; }
    public bool IsSelected { get; set; }

    public void UpdateSkillSlot(Skill skill)
    {
        _skillInSlot = skill;
        IsSelected = _skillInSlot.Selected;
        IsActive = _skillInSlot.Unlocked;
        _image.enabled = true;
        _image.sprite = _skillInSlot.Icon;
        RefreshUI();
    }

    public void RefreshUI ()
    {
        if (!IsActive)
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0.3f);
        else
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 1f);

        if (IsSelected)
        {
            _image.color = new Color(107, 191, 0, 1f);
            GetComponentInChildren<Button>().interactable = false;
        }
    }

    public void SelectSkill()
    {
        if (_skillInSlot.Unlocked)
        {
            _skillInSlot.Selected = true;
            UpdateSkillSlot(_skillInSlot);
        }
    }
}
