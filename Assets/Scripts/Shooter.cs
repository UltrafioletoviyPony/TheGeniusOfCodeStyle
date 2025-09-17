using System.Collections;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _velocity = 50;
    [SerializeField] private float _timeWaitShooting = .5f;

    private Coroutine _coroutine;
    private WaitForSeconds _waitForSeconds;

    private void Start()
    {
        _waitForSeconds = new WaitForSeconds(_timeWaitShooting);

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ShootRepeating());
    }

    private IEnumerator ShootRepeating()
    {
        bool isWork = true;

        while (isWork)
        {
            Vector3 bulletDirection = (_target.position - transform.position).normalized;
            GameObject bullet = Instantiate(_bulletPrefab, transform.position + bulletDirection, Quaternion.identity);

            if (bullet.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.transform.up = bulletDirection;
                rigidbody.linearVelocity = bulletDirection * _velocity;
            }
            
            bullet.transform.LookAt(_target.position);

            yield return _waitForSeconds;
        }
    }
}