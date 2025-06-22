using UnityEngine;
using UnityEngine.UI;

public class FactionDroneControl : MonoBehaviour
{
    [SerializeField] private Slider _droneCountSlider;
    [SerializeField] private SpeedSlider _speedSlider;
    [SerializeField] private SpawnerDrones _spawnerDrones;
    [SerializeField] private int _minDrones = 1;
    [SerializeField] private int _maxDrones = 5;

    private void Start()
    {
        _droneCountSlider.minValue = _minDrones;
        _droneCountSlider.maxValue = _maxDrones;
        _droneCountSlider.wholeNumbers = true; 

        _droneCountSlider.onValueChanged.AddListener(OnDroneCountSliderValueChanged);

        _droneCountSlider.value = _minDrones;
        UpdateDroneCount((int)_droneCountSlider.value);
    }

    private void OnDroneCountSliderValueChanged(float value) =>
        UpdateDroneCount((int)value);

    private void UpdateDroneCount(int targetDroneCount)
    {
        int currentActiveDroneCount = _spawnerDrones.GetActiveDroneCount();

        while (currentActiveDroneCount < targetDroneCount)
        {
            _spawnerDrones.SpawnDrone(_speedSlider.Speed);
            currentActiveDroneCount++;
        }

        while (currentActiveDroneCount > targetDroneCount)
        {
            _spawnerDrones.DestroyDrone();
            currentActiveDroneCount--;
        }
    }
}