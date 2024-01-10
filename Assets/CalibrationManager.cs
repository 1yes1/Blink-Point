using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrationManager : MonoBehaviour
{
    public static event Action OnCalibrationArrivedEvent;

    [SerializeField] private GameObject _calibration;

    [SerializeField] private List<Transform> _calibrationTransforms;

    [SerializeField] private float _calibrationMoveTime;

    [SerializeField] private float _calibrationWaitTime;

    private int _calibrationIndex;

    private void Start()
    {
        StartCoroutine(MovePoint());
    }


    private IEnumerator MovePoint()
    {
        PlayCalibrationAnimation();
        
        yield return new WaitForSeconds(_calibrationWaitTime);

        float time = 0;
        while (_calibrationIndex < _calibrationTransforms.Count)
        {
            Vector3 position = _calibrationTransforms[_calibrationIndex].transform.position;
            _calibration.transform.position = Vector3.Lerp(_calibration.transform.position, position, time / _calibrationMoveTime);
            time += Time.deltaTime;


            if (Vector3.Distance(_calibration.transform.position,position) <= 0.1f)
            {
                _calibrationIndex++;
                PlayCalibrationAnimation();
                time = 0;
                yield return new WaitForSeconds(_calibrationWaitTime);
                OnCalibrationArrivedEvent?.Invoke();
            }

            yield return null;
        }
    }

    private void PlayCalibrationAnimation()
    {
        if (_calibration.GetComponent<Animation>().isPlaying)
            _calibration.GetComponent<Animation>().Stop();

        _calibration.GetComponent<Animation>().Play();
    }

}
