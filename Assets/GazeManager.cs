using Mediapipe;
using Mediapipe.Unity;
using Mediapipe.Unity.IrisTracking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeManager : MonoBehaviour
{
    [SerializeField] private GameObject _eyeBall;
    private float[] _vector = new float[3];


    public void SetLandmarks(object stream, OutputEventArgs<NormalizedLandmarkList> eventArgs)
    {
        //print("Beklllee");

        //_vector[0] = landmarkList.Landmark[0].X;
        //_vector[1] = landmarkList.Landmark[0].Y;
        //_vector[2] = landmarkList.Landmark[0].Z;
    }

    private void Update()
    {

    }
}
