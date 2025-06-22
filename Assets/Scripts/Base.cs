using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Scanner _scanner;
    [SerializeField] private List<Drone> _drones;
    [SerializeField] private float _scanDelayTime = 1f;

    private SpawnerDrones _spawnerDrones;
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

    public void Initialize(ResourcesDatabase resourcesDatabase, SpawnerDrones spawnerDrones)
    {
        _resourcesDatabase = resourcesDatabase;
        _spawnerDrones = spawnerDrones;
    }

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
            Resource resource = _availableResources[0];
            _resourcesDatabase.ReserveResources(resource);
            drone.SendToResource(resource, drone);
            _availableResources.RemoveAt(0);
            drone.ResourceDelivered += OnResourceDelivered;
        }
    }

    private Drone GetFreeDrone() =>
       _drones.FirstOrDefault(drone => drone.IsBusy == false);
}