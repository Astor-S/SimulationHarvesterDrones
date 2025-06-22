using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Scanner _scanner;
    [SerializeField] private SpawnerDrones _spawnerDrones;
    [SerializeField] private List<Drone> _drones;
    [SerializeField] private float _scanDelayTime = 1f;

    private ResourcesDatabase _resourcesDatabase;
    private WaitForSeconds _scanDelay;
    private List<Resource> _availableResources = new();

    private int _resourceCount;

    public event Action ResourceCountChanged;

    public int ResourceCount => _resourceCount;

    private void Awake()
    {
        _scanDelay = new WaitForSeconds(_scanDelayTime);
    }

    private void OnEnable()
    {
        StartCoroutine(Working());
    }

    public void Initialize(ResourcesDatabase resourcesDatabase) =>
        _resourcesDatabase = resourcesDatabase;

    public void AddDrone(Drone drone) =>
       _drones.Add(drone);

    public void RemoveDrone(Drone droneToRemove) =>
       _drones.Remove(droneToRemove);

    private void OnResourceDelivered(Resource resource, Drone drone)
    {
        resource.Destroy();
        ReceiveResource();
        _resourcesDatabase.RemoveReservation(resource);
        drone.ResourceDelivered -= OnResourceDelivered;
    }

    private void ReceiveResource()
    {
        _resourceCount++;
        ResourceCountChanged?.Invoke();
    }

    private IEnumerator Working()
    {
        while (enabled)
        {
            yield return _scanDelay;

            _availableResources = _resourcesDatabase.GetFreeResources(_scanner.Scan()).ToList();
            SendDrone();
        }
    }

    private void SendDrone()
    {
        Drone freeDrone = GetFreeDrone();

        if (freeDrone != null)
            SendDronesForResources(freeDrone);
    }

    private void SendDronesForResources(Drone drone)
    {
        if (_availableResources.Count > 0)
        {
            Resource nearestResource = FindNearestResource(drone.transform.position);

            if (nearestResource != null)
            {
                _resourcesDatabase.ReserveResources(nearestResource);
                drone.SendToResource(nearestResource, drone);
                _availableResources.Remove(nearestResource);
                drone.ResourceDelivered += OnResourceDelivered;
            }
        }
    }

    private Resource FindNearestResource(Vector3 dronePosition)
    {
        Resource nearestResource = null;
        
        float minDistance = Mathf.Infinity;

        foreach (Resource resource in _availableResources)
        {
            float distance = Vector3.Distance(dronePosition, resource.transform.position);
            
            if (distance < minDistance)
            {
                nearestResource = resource;
                minDistance = distance;
            }
        }

        return nearestResource;
    }

    private Drone GetFreeDrone() =>
       _drones.FirstOrDefault(drone => drone.IsBusy == false);
}