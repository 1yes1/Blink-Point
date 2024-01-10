using BlinkPoints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCircle : MonoBehaviour
{
    private static EyeCircle _instance;
    [SerializeField] private Vector3 _target;
    [SerializeField] private Vector2 _littleOffset;
    [SerializeField] private Vector2 _multiplier;

    private Vector3 _lastPosition;

    public static EyeCircle Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;

        _lastPosition = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Point>(out Point component))
        {
            GameManager.Instance.IncreaseClickCount();
        }
    }

    private void Update()
    {
        //SetPosition(_target);
    }

    public void SetPosition(Vector3 position)
    {
        //print("DÝstance: " + Vector3.Distance(_lastPosition, position));

        //if (_lastPosition != Vector3.zero && Vector3.Distance(_lastPosition, position) >= 200)
        //{
        //    print("Büyükkkk");
        //    _lastPosition = Vector3.zero;
        //}
        //else
        {
            Vector2 target = Camera.main.ViewportToScreenPoint(position);
            //print("Ataaa: "+ target);

            transform.position = target;
            _lastPosition = target;
        }
    }

    public void SetNormal(Vector2 eyeGazeNormal)
    {
        eyeGazeNormal += _littleOffset;
        eyeGazeNormal.x *= -1;
        eyeGazeNormal = eyeGazeNormal * _multiplier;

        //eyeGazeNormal.y = Mathf.Round(eyeGazeNormal.y * 10000f) / 10000f;
        //eyeGazeNormal.x = Mathf.Round(eyeGazeNormal.x * 10000f) / 10000f;

        //print("EyeGazeNormal: " + eyeGazeNormal.y);
        SetPosition(new Vector3(eyeGazeNormal.x, eyeGazeNormal.y,0));
    }
}
