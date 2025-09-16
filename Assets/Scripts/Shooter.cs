using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Shooter : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _velocity = 50;
    [SerializeField] float _timeWaitShooting = .5f;

    private ObjectPool<Bullet> _bulletPool;
    private int _poolCapacity = 3;
    private int _poolMaxSize = 3;

    private void Awake()
    {
        _bulletPool = new ObjectPool<Bullet>(
            createFunc: () => CreateBullet(),
            actionOnGet: (bullet) => GetBullet(bullet),
            actionOnRelease: (bullet) => bullet.gameObject.SetActive(false),
            actionOnDestroy: (bullet) => DestroyBullet(bullet),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    void Start() =>
        StartCoroutine(ShootingWorker());

    private Bullet CreateBullet()
    {
        Bullet _bullet = Instantiate(_bulletPrefab);
        _bullet.Release += ReleaseBullet;

        return _bullet;
    }

    private void GetBullet()
    {
        if (_bulletPool.CountAll < _poolCapacity || _bulletPool.CountActive < _bulletPool.CountAll)
            _bulletPool.Get();
    }

    private void GetBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
        bullet.Init(transform.position, _target.position, _velocity);
    }

    private void ReleaseBullet(Bullet bullet) =>
        _bulletPool.Release(bullet);

    private void DestroyBullet(Bullet bullet)
    {
        bullet.Release -= ReleaseBullet;
        Destroy(bullet.gameObject);
    }

    private IEnumerator ShootingWorker()
    {
        bool isWork = true;

        while (isWork)
        {
            GetBullet();

            yield return new WaitForSeconds(_timeWaitShooting);
        }
    }
}