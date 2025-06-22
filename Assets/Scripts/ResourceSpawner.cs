using System.Collections;
using UnityEngine;

public class ResourceSpawner : Spawner<Resource>
{
    [SerializeField] private float _startDelay = 1f;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private float _spawnRadius = 25f;

    private WaitForSeconds _waitStartDelay;
    private WaitForSeconds _waitRepeatRate;

    protected override void Awake()
    {
        base.Awake();
        _waitStartDelay = new WaitForSeconds(_startDelay);
        _waitRepeatRate = new WaitForSeconds(_repeatRate);
    }

    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    protected override void OnSpawn(Resource obj)
    {
        base.OnSpawn(obj);
        obj.Destroyed += OnObjectDestroyed;
    }

    protected override void OnObjectDestroyed(Resource obj)
    {
        base.OnObjectDestroyed(obj);
        obj.Destroyed -= OnObjectDestroyed;
    }

    private IEnumerator SpawnCoroutine()
    {
        yield return _waitStartDelay;

        while (enabled)
        {
            SpawnAtRandomPoint();
            yield return _waitRepeatRate;
        }
    }

    private void SpawnAtRandomPoint()
    {
        Vector3 randomPosition;
        Vector3 spawnPosition;

        int zeroCoordinateY = 0;

        do
        {
            randomPosition = Random.insideUnitSphere * _spawnRadius;
            spawnPosition = transform.position + randomPosition;
        }
        while (spawnPosition.y < zeroCoordinateY);

        Spawn(spawnPosition);
    }
}