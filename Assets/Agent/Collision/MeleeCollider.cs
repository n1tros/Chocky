using UnityEngine;

public class MeleeCollider : MonoBehaviour
{
    private WeaponController _weapons = null;

    private void Start()
    {
        _weapons = GetComponentInParent<WeaponController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HitBox") && collision.GetComponentInParent<AgentController>().transform != transform)
        {
            _weapons.MeleeCollider.enabled = false;
            //TODO: The actual damage dealt will be the BaseDamage x buffs x skills
            collision.GetComponentInParent<Health>().TakeDamage(-_weapons.Current.BaseDamage);
            //TODO: Decide knockback force and apply it, this should be based on weapon damage + size of player
            Vector2 knockBackDir = (collision.transform.position - transform.position).normalized * 20;
            knockBackDir.y = 1f;
            collision.GetComponentInParent<Rigidbody2D>().AddForce(knockBackDir, ForceMode2D.Impulse);
        }
    }
}
