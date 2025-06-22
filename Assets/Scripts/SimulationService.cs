using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimulationService : MonoBehaviour
{
    [SerializeField] private Slider _speedSlider;
    [SerializeField] private List<Mover> _allMovers = new();
    [SerializeField] private ResourceSpawner _resourceSpawner;
    [SerializeField] private SpawnerDrones _spawnerBlueDrones;
    [SerializeField] private SpawnerDrones _spawnerRedDrones;
    [SerializeField] private TMP_InputField _resourceSpawnRateInput;

    private void Start()
    {
        _speedSlider.onValueChanged.AddListener(OnSpeedSliderValueChanged);
        _resourceSpawnRateInput.onEndEdit.AddListener(OnResourceSpawnRateChanged);
        _spawnerBlueDrones.DroneSpawned += AddMoverToList;
        _spawnerBlueDrones.DroneDestroyed += RemoveMoverToList;
        _spawnerRedDrones.DroneSpawned += AddMoverToList;
        _spawnerRedDrones.DroneDestroyed += RemoveMoverToList;
    }

    private void OnDisable()
    {
        _speedSlider.onValueChanged.RemoveListener(OnSpeedSliderValueChanged);
        _resourceSpawnRateInput.onEndEdit.RemoveListener(OnResourceSpawnRateChanged);
        _spawnerBlueDrones.DroneSpawned -= AddMoverToList;
        _spawnerBlueDrones.DroneDestroyed -= RemoveMoverToList;
        _spawnerRedDrones.DroneSpawned -= AddMoverToList;
        _spawnerRedDrones.DroneDestroyed -= RemoveMoverToList;
    }

    public void OnResourceSpawnRateChanged(string text)
    {
        if (float.TryParse(text, out float spawnRate))
            _resourceSpawner.UpdateRepeatRate(spawnRate);
        else
            _resourceSpawnRateInput.text = _resourceSpawner.RepeatRate.ToString();
    }

    private void OnSpeedSliderValueChanged(float speed)
    {
        foreach (Mover mover in _allMovers)
        {
            if (mover != null)
                mover.Speed = speed;
        }
    }

    private void AddMoverToList(Mover mover) =>
        _allMovers.Add(mover);

    private void RemoveMoverToList(Mover mover) =>
        _allMovers.Remove(mover);
}