using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private Transform _way;
    [SerializeField] private float _speed = 5f;

    private Transform[] _wayPointsTransforms;
    private int _currentWayPoint = 0;

    private void Start()
    {
        _wayPointsTransforms = new Transform[_way.childCount];

        for (int i = 0; i < _way.childCount; i++)
            _wayPointsTransforms[i] = _way.GetChild(i).transform;
    }

    private void Update()
    {
        Transform wayPointTransform = _wayPointsTransforms[_currentWayPoint];
        transform.position = Vector3.MoveTowards(transform.position, wayPointTransform.position, _speed * Time.deltaTime);

        if (transform.position == wayPointTransform.position) 
            UpdateCurrentWayPoint();
    }

    private void UpdateCurrentWayPoint()
    {
        _currentWayPoint++;

        if (_currentWayPoint == _wayPointsTransforms.Length)
            _currentWayPoint = 0;

        Vector3 forwardPointPosition = _wayPointsTransforms[_currentWayPoint].transform.position;
        transform.forward = forwardPointPosition - transform.position;
    }
}