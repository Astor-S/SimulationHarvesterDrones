using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerDrones : Spawner<Drone>
{
    private readonly List<Drone> _activeDrones = new();
    
    [SerializeField] private Base _base;
    [SerializeField] private Transform _spawnPoint;

    public event Action<Mover> DroneSpawned;
    public event Action<Mover> DroneDestroyed;

    public Drone SpawnDrone(float speed)
    {
        Drone drone = Spawn(_spawnPoint.position); 
        
        if (drone != null)
        {
            _activeDrones.Add(drone);
            _base.AddDrone(drone);
            Mover mover = drone.GetComponent<Mover>();
            mover.Speed = speed;
            DroneSpawned?.Invoke(drone.GetComponent<Mover>());
        }

        return drone;
    }

    public void DestroyDrone()
    {
        if (_activeDrones.Count > 0)
        {
            Drone droneToDestroy = _activeDrones[_activeDrones.Count - 1];
            _activeDrones.Remove(droneToDestroy);
            _base.RemoveDrone(droneToDestroy);
            DroneDestroyed?.Invoke(droneToDestroy.GetComponent<Mover>());
            droneToDestroy.gameObject.SetActive(false);
            base.OnObjectDestroyed(droneToDestroy);
        }
    }

    public int GetActiveDroneCount() => 
        _activeDrones.Count;

    protected override void OnObjectDestroyed(Drone obj)
    {
        base.OnObjectDestroyed(obj);
        obj.gameObject.SetActive(false);
        _activeDrones.Remove(obj);
    }

    protected override void OnSpawn(Drone obj)
    {
        base.OnSpawn(obj);
        obj.Init(_base);
    }
}