using UnityEngine;

public interface ILauncher
{
    GameObject BulletPrefab { get; set; }

    void Spawn(float bulletSpeed);
}