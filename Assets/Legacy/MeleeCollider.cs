using UnityEngine;

public class MeleeCollider : MonoBehaviour
{
    // TODO: find a way to load this effect from weapon
    [SerializeField]
    private GameObject _sprayPrefab, _bloodPrefab;
    private Weapons _weapons;

    private void Start()
    {
        _weapons = GetComponentInParent<Weapons>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HitBox") && collision.gameObject != transform.parent.gameObject)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            CreateParticleObject(collision);
            var hitRotation = (collision.transform.position - transform.position).normalized;
            collision.GetComponentInParent<IDamagable>().TakeDamage(_weapons.MeleeWeapon.CurrentDamage, _weapons.MeleeWeapon.WeaponForce, hitRotation);
        }
    }

    private void CreateParticleObject(Collider2D collision)
    {
        var rotation = _sprayPrefab.transform.rotation;

        var create = Instantiate(_sprayPrefab, collision.transform.parent.position, rotation, collision.transform.parent);
        var children = create.GetComponentsInChildren<Transform>();
        foreach (var item in children)
        {
            item.transform.localScale = _weapons.transform.localScale;
        }
        var x = Random.Range(-0.69f, 0.27f) + create.transform.position.x;
        var y = Random.Range(-1.5f, 0.5f) + create.transform.position.y;
        var randomSpawnPoint = new Vector3(x, y, create.transform.position.z);
        create.transform.position = randomSpawnPoint;
    }
}
