using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStats")]
public class Stats : ScriptableObject
{
    [SerializeField]
    private float _damage;
    public float Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    [SerializeField]
    private float _speed;
    public float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }

    [SerializeField]
    private float _fireRate;
    public float FireRate
    {
        set { _fireRate = value; }
        get { return _fireRate; }
    }

    [SerializeField]
    private float _abilityDamage;
    public float AbilityDamage
    {
        set { _abilityDamage = value; }
        get { return _abilityDamage; }
    }

    private void OnEnable()
    {
        _damage = 1f;
        _speed = 1f;
        _fireRate = 1f;
        _abilityDamage = 1f;
    }
}
