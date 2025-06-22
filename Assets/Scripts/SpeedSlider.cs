using UnityEngine;
using UnityEngine.UI;

public class SpeedSlider : MonoBehaviour
{
    [SerializeField] private Slider _slider; 
    [SerializeField] private float _minSpeed = 1f;
    [SerializeField] private float _maxSpeed = 10f;

    public float Speed { get; private set; }

    public delegate void SpeedChanged(float newSpeed);
    public event SpeedChanged OnSpeedChanged;

    private void Start()
    {
        _slider.minValue = _minSpeed;
        _slider.maxValue = _maxSpeed;

        _slider.onValueChanged.AddListener(OnSliderValueChanged);

        Speed = _minSpeed;
        _slider.value = Speed; 
        OnSpeedChanged?.Invoke(Speed); 
    }

    private void OnSliderValueChanged(float value)
    {
        Speed = value;
        OnSpeedChanged?.Invoke(Speed);
    }

    private void OnDestroy()
    {
        _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }
}