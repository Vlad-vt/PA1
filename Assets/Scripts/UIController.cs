using Assets.Scripts;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI EyeSeperationCurrentValue;

    public Slider EyeSepearationSlider;

    public TextMeshProUGUI FieldOfViewCurrentValue;

    public Slider FieldOfViewSlider;

    public TextMeshProUGUI NearClippingDistanceValue;

    public Slider NearClippingDistanceSlider;

    public TextMeshProUGUI ConvergenceDistanceValue;

    public Slider ConvergenceDistanceSlider;

    private void Start()
    {
        EyeSepearationSlider.onValueChanged.AddListener(OnEyeSeparationValueChanged);
        FieldOfViewSlider.onValueChanged.AddListener(OnFieldOfViewValueChanged);   
        NearClippingDistanceSlider.onValueChanged.AddListener(OnNearClippingDistanceValueChanged);
        ConvergenceDistanceSlider.onValueChanged.AddListener(OnConvergenceDistanceValueChanged);
        EyeSeperationCurrentValue.text = $"current value: 1";
        FieldOfViewCurrentValue.text = $"current value: 30";
        NearClippingDistanceValue.text = $"current value: 0.3";
        ConvergenceDistanceValue.text = $"current value: 1";
        EyeSepearationSlider.value = 1;
        FieldOfViewSlider.value = 60;
        NearClippingDistanceSlider.value = 0.3f;
        ConvergenceDistanceSlider.value = 1;
    }

    private void Update()
    {
        
    }

    private void OnEyeSeparationValueChanged(float value)
    {
        Debug.Log("Eye Separation value: " + value);
        EyeSeperationCurrentValue.text = $"current value: {value}";
        LabParameters.GetInstance().ChangeLabValue(value, Assets.Scripts.Enums.ValueType.EyeSeparation);
    }

    private void OnFieldOfViewValueChanged(float value)
    {
        Debug.Log("Field of view value: " + value);
        FieldOfViewCurrentValue.text = $"current value: {value}";
        LabParameters.GetInstance().ChangeLabValue(value, Assets.Scripts.Enums.ValueType.FildOfView);
    }

    private void OnNearClippingDistanceValueChanged(float value)
    {
        Debug.Log("Near clipping distance value: " + value);
        NearClippingDistanceValue.text = $"current value: {value}";
        LabParameters.GetInstance().ChangeLabValue(value, Assets.Scripts.Enums.ValueType.NearClipDistance);
    }

    private void OnConvergenceDistanceValueChanged(float value)
    {
        Debug.Log("Convergence distance value: " + value);
        ConvergenceDistanceValue.text = $"current value: {value}";
        LabParameters.GetInstance().ChangeLabValue(value, Assets.Scripts.Enums.ValueType.ConvergenceDistance);
    }
}
