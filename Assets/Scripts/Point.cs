using BlinkPoints;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class Point : MonoBehaviour
{
    private Image _image;

    private float _showDuration;
    private float _stayDuration;
    private float _hideDuration;
    private int _maxShowCount;
    private int _showCount;
    private int _clickCount;

    public int ShowCount => _showCount;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _showCount = 0;
    }
    public void HideAtStart()
    {
        Color color = _image.color;
        color.a = 0;
        _image.color = color;
    }

    public void Show(float showDuration,float stayDuration,float hideDuration,int maxShowCount)
    {
        _showDuration = showDuration;
        _stayDuration = stayDuration;
        _hideDuration = hideDuration;
        _maxShowCount = maxShowCount;
        StartCoroutine(IEShow());
     
        _showCount++;
    }

    public void Show()
    {
        int missCount = _maxShowCount - _clickCount;
        float increase = 1f / _maxShowCount;
        float val = 1 - (missCount * increase);

        Color color = _image.color;
        color.r = val;
        color.g = val;
        color.b = val;
        color.a = 1;
        _image.color = color;
    }

    public void IncreaseClickCount()
    {
        _clickCount++;
        //print("IncreaseClickCount");
    }

    private IEnumerator IEShow()
    {
        float alpha = 0;
        float time = 0;
        Color color = _image.color;

        while (time <= _showDuration)
        {
            alpha = time / _showDuration;
            color.a = alpha;
            _image.color = color;

            time += Time.deltaTime;
            yield return null;
        }

        color = _image.color;
        color.a = 1;
        _image.color = color;
        OnPointVisible();
    }

    private void OnPointVisible()
    {
        //print("Point Visible");
        Hide();
    }
    private void Hide()
    {
        StartCoroutine(IEHide(_stayDuration));
    }

    private IEnumerator IEHide(float delay)
    {
        yield return new WaitForSeconds(delay);

        float alpha = 1;
        float time = 0;
        Color color = _image.color;

        while (time <= _hideDuration)
        {
            alpha = 1 - (time / _hideDuration);
            color.a = alpha;
            _image.color = color;

            time += Time.deltaTime;
            //print("Time: " + time + " Alpha: " + alpha);

            yield return null;
        }

        color = _image.color;
        color.a = 0;
        _image.color = color;
        OnPointInvisible();
    }


    private void OnPointInvisible()
    {
        GameEventCaller.Instance.OnPointInvisible(this);
        //print("Point Invisible");
    }

}
