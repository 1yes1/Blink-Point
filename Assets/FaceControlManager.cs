using BlinkPoints;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FaceControlManager : MonoBehaviour
{
    [SerializeField] private GameObject _faceControl;
    [SerializeField] private TextMeshProUGUI _countdownText;

    [Header("Settings")]
    [SerializeField] private float _countDown;

    private void Start()
    {
        StartCoroutine(nameof(StartCountdown));
    }

    private IEnumerator StartCountdown()
    {
        float countdown = _countDown;
        while (countdown > 0)
        {
            countdown -= Time.deltaTime;
            _countdownText.text = (Mathf.RoundToInt(countdown)).ToString();
            yield return null;
        }
        _faceControl.SetActive(false);
        _countdownText.gameObject.SetActive(false);
        GameEventCaller.Instance.OnCountdownEnded();

    }
}
