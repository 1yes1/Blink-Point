using System.Collections;
using System.Collections.Generic;

using UnityEngine;
namespace BlinkPoints
{
    public class PointManager : MonoBehaviour
    {
        [SerializeField] private List<Point> _points;
        [SerializeField] private List<Point> _tempPoints;
        private float _timeBetweenBlinks = 1;
        private float _showDuration = 0.2f;
        private float _stayDuration = 0.2f;
        private float _hideDuration = 0.2f;
        private int _maxShowCount = 5;

        private bool _useRandomBlinkTimes = false;

        private IEnumerator Start()
        {
            _tempPoints = new List<Point>(_points);

            HidePoints();
            //TestStart();
            yield return new WaitForSeconds(1f);
            StartCoroutine(BlinkPoints());
        }

        private void TestStart()
        {
            _timeBetweenBlinks = 0.07f;
            _showDuration = 0.02f;
            _stayDuration = 0.02f;
            _hideDuration = 0.02f;
        }

        private void HidePoints()
        {
            for (int i = 0; i < _tempPoints.Count; i++)
            {
                _tempPoints[i].HideAtStart();
            }
        }

        private void ChooseRandomPoint()
        {
            int rnd = Random.Range(0, _tempPoints.Count);
            _tempPoints[rnd].Show(_showDuration, _stayDuration, _hideDuration,_maxShowCount);
            GameEventCaller.Instance.OnPointVisible(_tempPoints[rnd]);

            //int inc = Random.Range(0, 4);
            //if (inc > 0)
            //    _tempPoints[rnd].IncreaseClickCount();

            if (_tempPoints[rnd].ShowCount >= _maxShowCount)
                _tempPoints.RemoveAt(rnd);
        }

        private IEnumerator BlinkPoints()
        {
            GameEventCaller.Instance.OnTestStarted();

            while (_tempPoints.Count > 0)
            {
                ChooseRandomPoint();

                if(_useRandomBlinkTimes)
                    yield return new WaitForSeconds(Random.Range(_timeBetweenBlinks, _timeBetweenBlinks + 0.5f));
                else
                    yield return new WaitForSeconds(_timeBetweenBlinks);

            }
            GameEventCaller.Instance.OnCompleted();
            OnCompleted();
        }

        private void OnCompleted()
        {
            for (int i = 0; i < _points.Count; i++)
            {
                _points[i].Show();
            }
        }

    }

}
