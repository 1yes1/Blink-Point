using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    [SerializeField] private DefaultSettings _defaultSettings;
    [SerializeField] private List<Point> _points;
    [SerializeField] private float _timeBetweenBlinks = 1;
    [SerializeField] private float _showDuration;
    [SerializeField] private float _stayDuration;
    [SerializeField] private float _hideDuration;

    private void Awake()
    {
        SetValues();
    }

    private void Start()
    {
        HidePoints();
        StartCoroutine(BlinkPoints());
    }

    private void SetValues()
    {
        for (int i = 0; i < _defaultSettings.DefaultValues.Count; i++)
        {
            switch (_defaultSettings.DefaultValues[i].settingsPrefs)
            {
                case SettingsPrefs.TimeBetweenBlinks:
                    _timeBetweenBlinks = PlayerPrefs.GetFloat(_defaultSettings.DefaultValues[i].settingsPrefs.ToString());
                    break;
                case SettingsPrefs.ShowDuration:
                    _showDuration = PlayerPrefs.GetFloat(_defaultSettings.DefaultValues[i].settingsPrefs.ToString());
                    break;
                case SettingsPrefs.StayDuration:
                    _stayDuration = PlayerPrefs.GetFloat(_defaultSettings.DefaultValues[i].settingsPrefs.ToString());
                    break;
                case SettingsPrefs.HideDuration:
                    _hideDuration = PlayerPrefs.GetFloat(_defaultSettings.DefaultValues[i].settingsPrefs.ToString());
                    break;
                default:
                    break;
            }
        }
    }

    private void HidePoints()
    {
        for (int i = 0; i < _points.Count; i++)
        {
            _points[i].HideAtStart();
        }
    }

    private void ChooseRandomPoint()
    {
        int rnd = Random.Range(0, _points.Count);
        _points[rnd].Show(_showDuration, _stayDuration, _hideDuration);
    }

    private IEnumerator BlinkPoints()
    {
        while (true)
        {
            ChooseRandomPoint();
            yield return new WaitForSeconds(_timeBetweenBlinks);
        }
    }

}
