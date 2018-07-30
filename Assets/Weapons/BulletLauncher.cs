using UnityEngine;

public class BulletLauncher : MonoBehaviour, ILauncher
{
    [SerializeField]
    private Transform _bulletSpawnPointStanding, _bulletSpawnPointCrouching;

    private Agent _agent;

    private void Start()
    {
        _agent = GetComponent<Agent>();
    }

    public void AttackSpawn(GameObject bulletPrefab, Weapon weapon)
    {
        var bulletSpawnPoint = (_agent.StateMachine.CurrentState is CrouchState) ? _bulletSpawnPointCrouching.position : _bulletSpawnPointStanding.position;
        
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint, transform.rotation);
        bullet.GetComponent<Projectile>().Initialise(weapon.WeaponForce, weapon.CurrentDamage, tag);
        FireBullet(weapon.ProjectileVelocity, bullet);
    }

    private void FireBullet(float bulletSpeed, GameObject bulletGameObject)
    {
        var speed = bulletSpeed;

        if (bulletGameObject.transform.position.x < transform.position.x)
        {
            bulletGameObject.transform.localScale = new Vector3(-bulletGameObject.transform.localScale.x, bulletGameObject.transform.localScale.y, bulletGameObject.transform.localScale.z);
            speed = -bulletSpeed;
        }

        var rigid = bulletGameObject.GetComponent<Rigidbody2D>();
        rigid.velocity = new Vector2(speed, rigid.velocity.y);
    }
    /*
    public void Spawn(float bulletSpeed)
    {
        var bulletGameObject = CreateBulletObjectInPosition();
        CreateBulletAndSetProperties(bulletGameObject);
        FireBullet(bulletSpeed, bulletGameObject);
    }
    
    private GameObject CreateBulletObjectInPosition()
    {
        var position = _agentController.IsCrouched ? BulletSpawnPointCrouched.position : BulletSpawnPointStanding.position;
        return Instantiate(BulletPrefab, position, _weaponcontroller.transform.rotation);
    }
    

    private void CreateBulletAndSetProperties(GameObject bulletGameObject)
    {
        var projectile = bulletGameObject.GetComponent<Projectile>();

        projectile.KnockBackForce = _weaponcontroller.Current.KnockbackForce;
        projectile.Owner = _agentController.tag;
        projectile.Damage = -_weaponcontroller.Current.BaseDamage;
    }
    */
}


