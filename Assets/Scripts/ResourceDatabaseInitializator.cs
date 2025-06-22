using UnityEngine;

public class ResourceDatabaseInitializator : MonoBehaviour
{
    private readonly ResourcesDatabase _resourcesDatabase = new();

    [SerializeField] private Base _blueBase;
    [SerializeField] private Base _redBase;

    private void Start()
    {
        _blueBase.Initialize(_resourcesDatabase);
        _redBase.Initialize(_resourcesDatabase);
    }
}