using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Mediapipe.Unity.Tutorial.FaceMesh;

[Serializable]
public class EyePoint
{
    private int _index;
    private GameObject _gameObject;
    private Vector3 _worldPosition;
    private Vector3 _imagePosition;

    public int Index => _index;

    public GameObject GameObject
    {
        get { return _gameObject; }
        set { _gameObject = value; }
    }

    public Vector3 WorldPosition
    {
        get { return _worldPosition; }
        set { _worldPosition = value; }
    }

    public Vector3 ImagePosition
    {
        get { return _imagePosition; }
        set { _imagePosition = value; }
    }

    public EyePoint(int index)
    {
        _index = index;
    }
}
public enum EyeCorner
{
    LeftEyePupil = 473,
    LeftEyeLeftCorner = 362,
    LeftEyeRightCorner = 263,
    LeftEyeBottomCorner = 374,
    LeftEyeTopCorner = 386,
    RighEyePupil = 468,
    RightEyeLeftCorner =33,
    RightEyeRightCorner = 133,
    RightEyeBottomCorner = 145,
    RightEyeTopCorner = 158
}

//public struct EyePoint
//{
//    public GameObject EyePointObject;
//    public FaceLandmark FaceLandmark;

//    public EyePoint(GameObject eyePointObject, FaceLandmark faceLandmark)
//    {
//        EyePointObject = eyePointObject;
//        FaceLandmark = faceLandmark;
//    }

//    public void SetFaceLandmark(FaceLandmark faceLandmark)
//    {
//        FaceLandmark = faceLandmark;
//    }

//}

