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


    private void Awake()
    {
        _image = GetComponent<Image>();
    }
    public void HideAtStart()
    {
        Color color = _image.color;
        color.a = 0;
        _image.color = color;
    }

    public void Show(float showDuration,float stayDuration,float hideDuration)
    {
        _showDuration = showDuration;
        _stayDuration = stayDuration;
        _hideDuration = hideDuration;
        StartCoroutine(IEShow());
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

    private void OnPointVisible()
    {
        //print("Point Visible");
        Hide();
    }

    private void OnPointInvisible()
    {
        //print("Point Invisible");
    }

}
