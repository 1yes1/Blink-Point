using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EyeTargetController : MonoBehaviour
{
    private static EyeTargetController _instance;

    [SerializeField] private Vector2 _target;
    [SerializeField] private Vector2 _littleOffset;
    [SerializeField] private Vector2 _multiplier;

    private RectTransform _rectTransform;
    private Vector2 _lastPosition;

    public static EyeTargetController Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;

        _lastPosition = Vector2.zero;
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        //SetPosition(_target);
    }
    public void SetPosition(Vector2 vector2)
    {
        //print("DÝstance: " + Vector3.Distance(_lastPosition, vector2));

        if (_lastPosition != Vector2.zero && Vector3.Distance(_lastPosition, vector2) >= 2.5f)
        {
            //print("Büyükkk{k");

        }
        else
        {
            _rectTransform.anchoredPosition = Camera.main.ViewportToScreenPoint(vector2);
            _lastPosition = vector2;
        }
    }

    public void SetNormal(Vector2 eyeGazeNormal)
    {
        eyeGazeNormal += _littleOffset;
        eyeGazeNormal.x *= -1;
        eyeGazeNormal = eyeGazeNormal * _multiplier;
        SetPosition(eyeGazeNormal);
    }
}
