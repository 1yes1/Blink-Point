using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BlinkPoints
{
    public class SettingsItem : MonoBehaviour
    {
        [SerializeField] SettingsPrefs _settingsPrefs;
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _sliderText;

        private float _sliderValue = 0;

        private int _randomTime = 0; //Parlamalar arasý süreler random mu yoksa sabit mi olacak

        public string PrefsKey => _settingsPrefs.ToString();

        public SettingsPrefs SettingsPrefs => _settingsPrefs;

        public float SliderValue
        {
            get { return _sliderValue; }
            set
            {
                _sliderValue = value;
                if(_settingsPrefs == SettingsPrefs.RandomTime)
                    _sliderText.text = (_sliderValue == 1) ? "Evet" : "Hayýr";
                else
                    _sliderText.text = _sliderValue.ToString("0.00") + "s";

                _slider.value = _sliderValue;
                PlayerPrefs.SetFloat(PrefsKey, _sliderValue);
            }
        }

        private void OnEnable()
        {
            _slider?.onValueChanged.AddListener(OnValueChanged);
        }

        private void Start()
        {
            SetPrefs();
        }

        private void SetPrefs()
        {
            if (!PlayerPrefs.HasKey(PrefsKey))
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

}
