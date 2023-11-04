using System.Collections;
using System.Collections.Generic;

using UnityEngine;
namespace BlinkPoints
{
    public class PointManager : MonoBehaviour
    {
        [SerializeField] private List<Point> _points;
        private float _timeBetweenBlinks = 1;
        private float _showDuration = 0.2f;
        private float _stayDuration = 0.2f;
        private float _hideDuration = 0.2f;

        private bool _useRandomBlinkTimes = false;

        private IEnumerator Start()
        {
            HidePoints();

            yield return new WaitForSeconds(1f);
            StartCoroutine(BlinkPoints());
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
