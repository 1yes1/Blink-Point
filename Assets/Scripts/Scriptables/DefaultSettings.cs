using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlinkPoints
{
    [CreateAssetMenu(fileName = "DefaultSettings", menuName = "ScriptableObjects/DefaultSettings", order = 1)]
    public class DefaultSettings : ScriptableObject
    {
        public List<SettingsPrefsItem> DefaultValues;
    }

    [Serializable]
    public class SettingsPrefsItem
    {
        public SettingsPrefs settingsPrefs;
        public float defaultValue;
    }

}
