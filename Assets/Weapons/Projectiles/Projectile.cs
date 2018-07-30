using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float _knockBackForce;
    private float _damage;
    private string _parentTag;

    [SerializeField] GameObject _sprayPrefab;

    public void Initialise(float knockBackForce, float damage, string parentTag)
    {
        _knockBackForce = knockBackForce;
        _damage = damage;
        _parentTag = parentTag;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HitBox") && !collision.transform.parent.gameObject.CompareTag(_parentTag))
        {
            Vector2 knockBackDir = (collision.transform.position - transform.position).normalized;
            CreateParticleObject(collision);
            collision.GetComponentInParent<IDamagable>().TakeDamage(_damage, _knockBackForce, knockBackDir);
            Destroy(gameObject);
        }
    }

    private void CreateParticleObject(Collider2D collision)
    {
        var rotation = _sprayPrefab.transform.rotation;

        var create = Instantiate(_sprayPrefab, collision.transform.parent.position, rotation, collision.transform.parent);
        var children = create.GetComponentsInChildren<Transform>();
        foreach (var item in children)
        {
            item.transform.localScale = transform.localScale;
        }
        var x = Random.Range(-0.69f, 0.27f) + create.transform.position.x;
        var y = 1.2f + create.transform.position.y;
        var randomSpawnPoint = new Vector3(x, y, create.transform.position.z);
        create.transform.position = randomSpawnPoint;
    }
}
//    [SerializeField] float _bulletSpeed = 30f;

//    public float Damage { get; set; }
//    public Rigidbody2D Rigid { get; set; }
//    public float BulletSpeed { get { return _bulletSpeed; } set { _bulletSpeed = value; } }
//    public string Owner { get; set; }
//    public float KnockBackForce { get; set; }

//    // Use this for initialization
//    void Start()
//    {
//        Rigid = GetComponent<Rigidbody2D>();
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("HitBox") && Owner != collision.GetComponentInParent<WeaponController>().tag)
//        {
//            collision.GetComponentInParent<Health>().TakeDamage(Damage);

//            Vector2 knockBackDir = (collision.transform.position - transform.position).normalized * KnockBackForce;
//            knockBackDir.y = 1f;
//            Destroy(gameObject);
//        }

//        // TODO: Draw an array of layers bullet should be destroyed by.
//        else if (collision.CompareTag("Ground") || collision.CompareTag("Wall"))
//        {
//            Destroy(gameObject);
//        }
//    }
//}
