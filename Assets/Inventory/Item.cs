using UnityEngine;

public class Item : ScriptableObject
{
    [SerializeField]
    private Sprite _icon;
    public Sprite Icon { get { return _icon; } }
}
