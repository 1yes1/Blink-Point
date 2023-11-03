using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private DefaultSettings _defaultSettings;
    [SerializeField] private List<SettingsItem> _items;


    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            PlayerPrefs.SetFloat(_items[i].PrefsKey, GetPrefsValue(_items[i].SettingsPrefs, _items[i].PrefsKey));
        }
    }

    private float GetPrefsValue(SettingsPrefs settingsPrefs,string prefsKey,bool defaultValue = false)
    {
        for (int i = 0; i < _defaultSettings.DefaultValues.Count; i++)
        {
            if (settingsPrefs == _defaultSettings.DefaultValues[i].settingsPrefs)
            {
                float value = (PlayerPrefs.HasKey(prefsKey) && !defaultValue) ? PlayerPrefs.GetFloat(prefsKey) : _defaultSettings.DefaultValues[i].defaultValue;
                return value;
            }
        }

        return 0;
    }

    public void SetDefaultValues()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            float value = GetPrefsValue(_items[i].SettingsPrefs, _items[i].PrefsKey, true);
            PlayerPrefs.SetFloat(_items[i].PrefsKey, value);
            _items[i].SliderValue = value;
        }
    }

    public void OpenSettings()
    {
        _settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        _settingsPanel.SetActive(false);
    }

}

public enum SettingsPrefs
{
    TimeBetweenBlinks,
    ShowDuration,
    StayDuration,
    HideDuration
}
