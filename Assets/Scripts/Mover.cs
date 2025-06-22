using System;
using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private DroneAvoidance _droneAvoidance;
    [SerializeField] private float _speed = 1f;

    private Transform _targetTransform;
    private Coroutine _moveCoroutine;

    public event Action<float> SpeedChanged;

    public float Speed
    {
        get => _speed;
        set
        {
            if (_speed != value)
            {
                _speed = value;
                SpeedChanged?.Invoke(_speed);
            }
        }
    }

    public void MoveTo(Transform target)
    {
        _targetTransform = target;

        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);

        _moveCoroutine = StartCoroutine(MoveTowardsTarget());
    }

    private IEnumerator MoveTowardsTarget()
    {
        while (_targetTransform != null)
        {
            Vector3 avoidanceDirection = _droneAvoidance.GetAvoidanceDirection();
            Vector3 targetPosition = _targetTransform.position;
            Vector3 directionToTarget = (targetPosition - transform.position).normalized;
            Vector3 finalDirection = (directionToTarget + avoidanceDirection).normalized;

            transform.position = Vector3.MoveTowards(transform.position, transform.position + finalDirection, _speed * Time.deltaTime);

            yield return null;
        }
    }
}