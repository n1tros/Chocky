using UnityEngine;

public interface IDamagable
{
    void TakeDamage(float amount, float forceAmount, Vector2 forceDirection);
}
