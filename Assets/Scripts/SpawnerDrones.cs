using UnityEngine;

public class SpawnerDrones : Spawner<Drone>
{
    [SerializeField] private Base _base;

    protected override void OnSpawn(Drone obj) =>
        obj.Init(_base);
}