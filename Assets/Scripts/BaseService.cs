using UnityEngine;

public class BaseService : MonoBehaviour
{
    private readonly ResourcesDatabase _resourcesDatabase = new();

    [SerializeField] private Base _blueBase;
    [SerializeField] private Base _redBase;
    [SerializeField] private SpawnerDrones _spawnerDrones;

    private void Start()
    {
        _blueBase.Initialize(_resourcesDatabase, _spawnerDrones);
        _redBase.Initialize(_resourcesDatabase, _spawnerDrones);
    }
}