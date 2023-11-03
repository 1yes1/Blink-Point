using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsItem : MonoBehaviour
{
    [SerializeField] SettingsPrefs _settingsPrefs;
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _sliderText;

    private DefaultSettings _settings;
    private float _sliderValue = 0;

    public string PrefsKey => _settingsPrefs.ToString();
    public SettingsPrefs SettingsPrefs => _settingsPrefs;

    public float SliderValue
    {
        get { return _sliderValue; }
        set
        {
            _sliderValue = value;
            _sliderText.text = _sliderValue.ToString("0.00") + "s";
            _slider.value = _sliderValue;
            PlayerPrefs.SetFloat(PrefsKey, _sliderValue);
        }
    }

    private void OnEnable()
    {
        _slider.onValueChanged.AddListener(OnValueChanged);
    }

    private void Start()
    {
        SetPrefs();
    }

    private void SetPrefs()
    {
        if(!PlayerPrefs.HasKey(PrefsKey))
        {
            PlayerPrefs.SetFloat(PrefsKey, _sliderValue);
        }
        _slider.value = PlayerPrefs.GetFloat(PrefsKey);
        SliderValue = _slider.value;
    }

    private void OnValueChanged(float call)
    {
        SliderValue = call;
    }
}
