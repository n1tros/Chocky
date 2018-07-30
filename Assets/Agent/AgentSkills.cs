using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSkills : MonoBehaviour
{
    public event Action OnUnlockSKill = delegate { };

    [SerializeField]
    private List<Skill> _skillTree;
    public List<Skill> SkillTree
    {
        get { return _skillTree; }
        private set { _skillTree = value; }
    }

    private XP _xp;
    private Agent _agent;

    private void Awake()
    {
        _agent = GetComponent<Agent>();
        _xp = GetComponent<XP>();
        _xp.OnGainXP += CheckSkillUnlock;
        CheckSkillUnlock(0);
    }

    public void ApplyAllSkills()
    {
        _skillTree.ForEach(x =>
        {
            x.ResetSkill(_agent);

            if (x.Selected)
                x.AddSkill(_agent);
        });
    }

    public void CheckSkillUnlock (int XPAmount)
    {
        _skillTree.ForEach(x =>
        {
            if (x.XPToUnlock <= _xp.TotalXP)
            {
                x.Unlocked = true;
                OnUnlockSKill();
            }
            else
                x.Unlocked = false;
        });
    }

    public void SelectSkill (int position)
    {
        _skillTree[position].Selected = true;
    }

    private void OnDisable()
    {
        _xp.OnGainXP -= CheckSkillUnlock;
    }
}
