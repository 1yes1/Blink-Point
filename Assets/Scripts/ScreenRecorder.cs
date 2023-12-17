using BlinkPoints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenRecorder : RecorderBase
{
    [SerializeField] private RenderTexture _renderTexture;


    private void OnEnable()
    {
        GameEventReceiver.OnTestStartedEvent += OnTestStarted;
        GameEventReceiver.OnCompletedEvent += OnCompleted; ;
    }

    private void OnDisable()
    {
        GameEventReceiver.OnTestStartedEvent -= OnTestStarted;
        GameEventReceiver.OnCompletedEvent -= OnCompleted; ;
    }



    public override IEnumerator OnStart()
    {
        yield return null;
    }


    private void Update()
    {
        if (CanTakeSnapshot)
        {
            TakeSnapshot(ToTexture2D(_renderTexture));

            if (_startTime + _duration < Time.time)
            {
                SaveAsPNG("Screen");
            }
        }
    }


    private void OnTestStarted()
    {
        StartRecorder();
    }

    private void OnCompleted()
    {
        StopRecorder();
    }

    Texture2D ToTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(_width, _height, TextureFormat.RGB24, false);
        // ReadPixels looks at the active RenderTexture.
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }
}
