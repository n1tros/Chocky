//using UnityEngine;

//public class BulletSpawner : MonoBehaviour, ILauncher
//{
//    private WeaponController _weaponcontroller;
//    private AgentAction _agentController;

//    [SerializeField]
//    private Transform _bulletSpawnPointStanding = null;
//    public Transform BulletSpawnPointStanding
//    {
//        get { return _bulletSpawnPointStanding; }
//        set { _bulletSpawnPointStanding = value; }
//    }

//    [SerializeField]
//    private Transform _bulletSpawnPointCrouched = null;
//    public Transform BulletSpawnPointCrouched
//    {
//        get { return _bulletSpawnPointCrouched; }
//        set { _bulletSpawnPointCrouched = value; }
//    }
//    [SerializeField]
//    private GameObject _bulletPrefab = null;
//    public GameObject BulletPrefab
//    {
//        get { return _bulletPrefab; }
//        set { _bulletPrefab = value; }
//    }

//    private void Start()
//    {
//        _weaponcontroller = GetComponent<WeaponController>();
//        _agentController = GetComponent<AgentAction>();
//    }

//    public void Spawn(float bulletSpeed)
//    {
//        var bulletGameObject = CreateBulletObjectInPosition();
//        CreateBulletAndSetProperties(bulletGameObject);
//        FireBullet(bulletSpeed, bulletGameObject);
//    }

//    private GameObject CreateBulletObjectInPosition()
//    {
//        var position = _agentController.IsCrouched ? BulletSpawnPointCrouched.position : BulletSpawnPointStanding.position;
//        return Instantiate(BulletPrefab, position, _weaponcontroller.transform.rotation);
//    }

//    private void CreateBulletAndSetProperties(GameObject bulletGameObject)
//    {
//        var projectile = bulletGameObject.GetComponent<Projectile>();

//        projectile.KnockBackForce = _weaponcontroller.Current.KnockbackForce;
//        projectile.Owner = _agentController.tag;
//        projectile.Damage = -_weaponcontroller.Current.BaseDamage;
//    }

//    private void FireBullet(float bulletSpeed, GameObject bulletGameObject)
//    {
//        var speed = bulletSpeed;

//        if (bulletGameObject.transform.position.x < _agentController.transform.position.x)
//        {
//            bulletGameObject.transform.localScale = new Vector3(-bulletGameObject.transform.localScale.x, bulletGameObject.transform.localScale.y, bulletGameObject.transform.localScale.z);
//            speed = -bulletSpeed;
//        }

//        var rigid = bulletGameObject.GetComponent<Rigidbody2D>();
//        rigid.velocity = new Vector2(speed, rigid.velocity.y);
//    }
//}
