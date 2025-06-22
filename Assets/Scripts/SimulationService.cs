using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationService : MonoBehaviour
{
    [SerializeField] private Slider _speedSlider;
    [SerializeField] private List<Mover> _allMovers = new();

    private void Start()
    {
        _speedSlider.onValueChanged.AddListener(OnSpeedSliderValueChanged);
    }

    private void OnDisable()
    {
        _speedSlider.onValueChanged.RemoveListener(OnSpeedSliderValueChanged);
    }

    private void OnSpeedSliderValueChanged(float speed)
    {
        foreach (Mover mover in _allMovers)
        {
            if (mover != null)
                mover.Speed = speed;
        }
    }
}