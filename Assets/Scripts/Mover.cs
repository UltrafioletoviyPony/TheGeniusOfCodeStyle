using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private Transform _way;
    [SerializeField] private float _speed = 5f;

    private Vector3[] _wayPointPositions;
    private int _currentWayPoint = 0;

    Coroutine _coroutine;

    void Start()
    {
        if (_way.childCount > 0)
        {
            _wayPointPositions = new Vector3[_way.childCount];

            for (int i = 0; i < _way.childCount; i++)
                _wayPointPositions[i] = _way.GetChild(i).position;

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(Move());
        }
    }

    private IEnumerator Move()
    {
        bool isMoving = true;

        while (isMoving)
        {
            if (transform.position == _wayPointPositions[_currentWayPoint])
                _currentWayPoint = (_currentWayPoint + 1) % _wayPointPositions.Length;

            transform.position = Vector3.MoveTowards(transform.position, _wayPointPositions[_currentWayPoint], _speed * Time.deltaTime);
            transform.LookAt(_wayPointPositions[_currentWayPoint]);

            yield return null;
        }
    }
}