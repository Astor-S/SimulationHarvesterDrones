using UnityEngine;

public class DroneAvoidance : MonoBehaviour
{
    [SerializeField] private Mover _mover;
    [SerializeField] private float _scanDistance = 2f; 
    [SerializeField] private float _avoidanceForce = 1f; 
    [SerializeField] private LayerMask _obstacleLayer;

    private Vector3 _avoidanceDirection = Vector3.zero;

    private void Update()
    {
        _avoidanceDirection = Vector3.zero; 

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _scanDistance, _obstacleLayer))
        {
            Vector3 hitNormal = hit.normal;
            _avoidanceDirection = Vector3.Cross(transform.up, hitNormal).normalized * _avoidanceForce;
            Debug.DrawRay(transform.position, _avoidanceDirection * _scanDistance, Color.red);
        }
    }

    public Vector3 GetAvoidanceDirection() =>
        _avoidanceDirection;
}