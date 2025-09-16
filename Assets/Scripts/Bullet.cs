using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vector3 _bulletDirection;
    private Coroutine _coroutine;

    public event Action<Bullet> Release;

    private void Awake() =>
        _rigidbody = GetComponent<Rigidbody>();

    public void Init(Vector3 shootingPosition, Vector3 targetPosition, float velocity)
    {
        transform.position = shootingPosition;
        transform.LookAt(targetPosition);

        _bulletDirection = (targetPosition - shootingPosition).normalized;
        _rigidbody.linearVelocity = _bulletDirection * velocity;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        float currentTime = 0f;
        float endTime = 2f;

        while (currentTime < endTime)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }

        Release?.Invoke(this);
    }
}