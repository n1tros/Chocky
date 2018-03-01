using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float _bulletSpeed = 30f;

    public float Damage { get; set; }
    public Rigidbody2D Rigid { get; set; }
    public float BulletSpeed { get { return _bulletSpeed; } set { _bulletSpeed = value; } }
    public string Owner { get; set; }
    public float KnockBackForce { get; set; }

    // Use this for initialization
    void Start ()
    {
        Rigid = GetComponent<Rigidbody2D>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HitBox") && Owner != collision.GetComponentInParent<WeaponController>().tag)
        {
            collision.GetComponentInParent<AgentController>().TakeDamage(Damage);
            Vector2 knockBackDir = (collision.transform.position - transform.position).normalized * KnockBackForce;
            knockBackDir.y = 1f;
            Destroy(gameObject);
        } 

        // TODO: Draw an array of layers bullet should be destroyed by.
        else if (collision.CompareTag("Ground") || collision.CompareTag("Wall"))
        {
            Debug.Log("true");
            Destroy(gameObject);
        }
    }
}
