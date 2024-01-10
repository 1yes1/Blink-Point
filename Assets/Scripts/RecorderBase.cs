using BlinkPoints;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public abstract class RecorderBase : MonoBehaviour
{

#if UNITY_EDITOR 

    private string _savePath = "C:/BlinkPointsRecords/Videos";
    private string _saveFramePath = "C:/BlinkPointsRecords/Frames/";
#else
    private string _savePath = Application.persistentDataPath;
    private string _saveFramePath = Application.persistentDataPath;
#endif

    [SerializeField] protected int _width = 640;
    [SerializeField] protected int _height = 480;
    [SerializeField] protected int _fps = 15;

    protected List<Texture2D> _savedTextures;
    private int _captureCounter = 0;

    private bool _canTakeSnapshot = false;

    public bool CanTakeSnapshot => _canTakeSnapshot;

    protected float _startTime;
    protected float _duration = 12;



    public virtual IEnumerator Start()
    {
        _savedTextures = new List<Texture2D>();

        ClearOldFrames();

        yield return OnStart();

    }

    public abstract IEnumerator OnStart();


    private void ClearOldFrames()
    {
        DirectoryInfo d = new DirectoryInfo(_saveFramePath); //Assuming Test is your Folder

        FileInfo[] Files = d.GetFiles("*.png"); //Getting Text files
        string str = "";

        foreach (FileInfo file in Files)
        {
            File.Delete(file.FullName);
        }
    }

    protected void TakeSnapshot(Texture2D texture)
    {
        //return;
        _savedTextures.Add(texture);
        ++_captureCounter;

        Resources.UnloadUnusedAssets();
    }

    protected void SaveAsPNG(string preName)
    {
        _canTakeSnapshot = false;
        for (int i = 0; i < _savedTextures.Count; i++)
        {
            System.IO.File.WriteAllBytes(_saveFramePath + "/" + preName + "_" + i.ToString() + ".png", _savedTextures[i].EncodeToPNG());
        }
        print(preName + " Saved as PNG! => "+ _savedTextures.Count);
    }

    protected void StartRecorder()
    {
        _canTakeSnapshot = true;
        _startTime = Time.time;
    }

    protected void StopRecorder()
    {
        _canTakeSnapshot = false;
    }

}
