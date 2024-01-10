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

        private void OnEnable()
        {
            GameEventReceiver.OnCountdownEndedEvent += OnCountdownEnded;
        }

        private void OnDisable()
        {
            GameEventReceiver.OnCountdownEndedEvent -= OnCountdownEnded;
        }

        private void Start()
        {
            _tempPoints = new List<Point>(_points);

            HidePoints();
            //TestStart();
        }

        private void OnCountdownEnded()
        {
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

        private Point ChooseRandomPoint()
        {
            int rnd = Random.Range(0, _tempPoints.Count);
            Point point = _tempPoints[rnd];
            point.Show(_showDuration, _stayDuration, _hideDuration,_maxShowCount);
            GameEventCaller.Instance.OnPointVisible(point);

            //int inc = Random.Range(0, 4);
            //if (inc > 0)
            //    _tempPoints[rnd].IncreaseClickCount();

            if (point.ShowCount >= _maxShowCount)
                _tempPoints.RemoveAt(rnd);

            return point;
        }

        private IEnumerator BlinkPoints()
        {
            GameEventCaller.Instance.OnTestStarted();

            while (_tempPoints.Count > 0)
            {
                Point point = ChooseRandomPoint();

                if(_useRandomBlinkTimes)
                    yield return new WaitForSeconds(Random.Range(_timeBetweenBlinks, _timeBetweenBlinks + 0.5f));
                else
                    yield return new WaitForSeconds(_timeBetweenBlinks);

                GameEventCaller.Instance.OnBeforePointInvisible(point);

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
