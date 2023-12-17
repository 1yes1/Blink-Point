using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using BlinkPoints;

public class CameraRecorder : RecorderBase
{
    [SerializeField] protected RawImage _renderTexture;
    protected WebCamTexture _webCamTexture;

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
        if (WebCamTexture.devices.Length == 0)
        {
            throw new System.Exception("Web Camera devices are not found");
        }

#if UNITY_EDITOR
        var webCamDevice = WebCamTexture.devices[0];
#else
        var webCamDevice = WebCamTexture.devices[1];
#endif
        _webCamTexture = new WebCamTexture(webCamDevice.name, _width, _height, _fps);
        _webCamTexture.Play();

        yield return new WaitUntil(() => _webCamTexture.width > 16);

        _renderTexture.texture = _webCamTexture;
    }

    private void Update()
    {
        if (CanTakeSnapshot)
        {
            CreateTexture2D();

            if(_startTime + _duration < Time.time)
            {
                SaveAsPNG("Camera");
            }
        }
    }

    private void CreateTexture2D()
    {
        Texture2D snap = new Texture2D(_webCamTexture.width, _webCamTexture.height);
        snap.SetPixels(_webCamTexture.GetPixels());
        snap.Apply();

        TakeSnapshot(snap);
    }

    private void OnTestStarted()
    {
        StartRecorder();
    }

    private void OnCompleted()
    {
        StopRecorder();
    }

}
