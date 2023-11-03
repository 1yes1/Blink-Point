using System.Collections;
using System.Collections.Generic;

using UnityEngine;
namespace BlinkPoints
{
    public class PointManager : MonoBehaviour
    {
        [SerializeField] private DefaultSettings _defaultSettings;
        [SerializeField] private List<Point> _points;
        [SerializeField] private float _timeBetweenBlinks = 1;
        [SerializeField] private float _showDuration;
        [SerializeField] private float _stayDuration;
        [SerializeField] private float _hideDuration;

        private bool _useRandomBlinkTimes = false;

        private void Awake()
        {
            SetValues();
        }

        private IEnumerator Start()
        {
            HidePoints();

            yield return new WaitForSeconds(1f);
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
                    case SettingsPrefs.RandomTime:
                        _useRandomBlinkTimes = (PlayerPrefs.GetFloat(_defaultSettings.DefaultValues[i].settingsPrefs.ToString()) < 1) ? false : true;
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
            GameEventCaller.Instance.OnPointVisible(_points[rnd]);
            _points.RemoveAt(rnd);
        }

        private IEnumerator BlinkPoints()
        {
            while (_points.Count > 0)
            {
                ChooseRandomPoint();

                if(_useRandomBlinkTimes)
                    yield return new WaitForSeconds(Random.Range(_timeBetweenBlinks, _timeBetweenBlinks + 0.5f));
                else
                    yield return new WaitForSeconds(_timeBetweenBlinks);

            }
            GameEventCaller.Instance.OnCompleted();
        }

    }

}
