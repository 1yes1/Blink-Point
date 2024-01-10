using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity.CoordinateSystem;
using OpenCvSharp;

using Stopwatch = System.Diagnostics.Stopwatch;
using TMPro;
using System.Drawing;
using UnityEngine.Device;
using System.Linq;
using UnityEngine.XR;
using System;
using static Mediapipe.RenderAnnotation.Types;
using System.Diagnostics;
using Application = UnityEngine.Application;

namespace Mediapipe.Unity.Tutorial
{
    public class FaceMesh : MonoBehaviour
    {
        [SerializeField] private TextAsset _configAsset;
        [SerializeField] private RawImage _screen;
        [SerializeField] private MultiFaceLandmarkListAnnotationController _multiFaceLandmarksAnnotationController;
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private int _fps;

        [SerializeField] private GameObject _circlePrefab;
        [SerializeField] private List<int> _leftEyeLandmarksIndexes;
        [SerializeField] private List<int> _rightEyeLandmarksIndexes;
        [SerializeField] private List<int> _faceLandmarksIndexes;
        [SerializeField] private int _createFaceCircleCount;

        [SerializeField] private int _pupilLandmarkIndex;
        [SerializeField] private Vector2 _pupilDecreaseGap;
        [SerializeField] private Vector2 _pupilEffectMultiplier;

        //[Header("Eye Gaze Calibration")]
        //[SerializeField] private List<Vector2> _eyeGazeCalibrations;

        private List<GameObject> _faceCircles = new List<GameObject>();
        private List<GameObject> _leftEyeCircles = new List<GameObject>();
        private List<GameObject> _rightEyeCircles = new List<GameObject>();

        private Canvas _canvas;
        private CalculatorGraph _graph;
        private ResourceManager _resourceManager;

        private WebCamTexture _webCamTexture;
        private Texture2D _inputTexture;
        private Color32[] _inputPixelData;

        Vector3 frontVector = new Vector3(0, 0, 1);
        private List<FaceLandmark> _calculatedFaceLandmarks;

        private List<EyePoint> _leftEyePoints;
        private List<EyePoint> _rightEyePoints;

        private Stopwatch stopwatch;
        private OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>> multiFaceLandmarksStream;
        private UnityEngine.Rect screenRect;

        private bool _canDetectFace = false;
        private Vector2 _eyeGaze;

        private void Awake()
        {
            _canvas = FindObjectOfType<Canvas>();
        }

        private IEnumerator Start()
        {
            Application.targetFrameRate = 60;
            _calculatedFaceLandmarks = new List<FaceLandmark>();
            _leftEyePoints = new List<EyePoint>();
            _rightEyePoints = new List<EyePoint>();
            //for (int i = 0; i < _circleCount; i++)
            //{
            //    GameObject circle = Instantiate(_eyeBall, _eyeBall.transform.parent);
            //    _circles.Add(circle);
            //}

            for (int i = 0; i < _faceLandmarksIndexes.Count; i++)
            {
                GameObject circle = Instantiate(_circlePrefab, _canvas.transform);
                circle.name = "Face Landmark Circle (" + i + ")";
                _faceCircles.Add(circle);
            }
            for (int i = 0; i < _leftEyeLandmarksIndexes.Count; i++)
            {
                GameObject circle = Instantiate(_circlePrefab, _canvas.transform);
                circle.name = "Left Eye Circle (" + i + ")";
                _leftEyeCircles.Add(circle);
                _leftEyePoints.Add(new EyePoint(_leftEyeLandmarksIndexes[i]));
            }

            for (int i = 0; i < _rightEyeLandmarksIndexes.Count; i++)
            {
                GameObject circle = Instantiate(_circlePrefab, _canvas.transform);
                circle.name = "Right Circle (" + i + ")";
                _rightEyeCircles.Add(circle);
                _rightEyePoints.Add(new EyePoint(_rightEyeLandmarksIndexes[i]));
            }

            if (WebCamTexture.devices.Length == 0)
            {
                throw new System.Exception("Web Camera devices are not found");
            }

            var webCamDevice = WebCamTexture.devices[0];

            _webCamTexture = new WebCamTexture(webCamDevice.name, _width, _height, _fps);
            _webCamTexture.Play();

            yield return new WaitUntil(() => _webCamTexture.width > 16);

            _screen.rectTransform.sizeDelta = new Vector2(_width, _height);
            _screen.rectTransform.sizeDelta = new Vector2(_width, _height);

            _inputTexture = new Texture2D(_width, _height, TextureFormat.RGBA32, false);
            _inputPixelData = new Color32[_width * _height];

            _screen.texture = _webCamTexture;
            _screen.texture = _webCamTexture;

            _resourceManager = new StreamingAssetsResourceManager();
            yield return _resourceManager.PrepareAssetAsync("face_detection_short_range.bytes");
            yield return _resourceManager.PrepareAssetAsync("face_landmark_with_attention.bytes");

            stopwatch = new Stopwatch();

            _graph = new CalculatorGraph(_configAsset.text);
            multiFaceLandmarksStream = new OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>>(_graph, "multi_face_landmarks");
            multiFaceLandmarksStream.StartPolling().AssertOk();
            _graph.StartRun().AssertOk();
            stopwatch.Start();

            screenRect = _screen.GetComponent<RectTransform>().rect;
            _canDetectFace = true;

            
        }

        private void LateUpdate()
        {
            DetectFaceMesh();

            if (_leftEyeCircles.Count > 0)
            {
                //Vector3 nose = _faceCircles[0].GetComponent<RectTransform>().TransformPoint(_faceCircles[0].GetComponent<RectTransform>().anchoredPosition);

                Vector3 leftPupil = GetEyeCornerWorldPosition(EyeCorner.LeftEyePupil);
                Vector3 leftEyeLeftCorner = GetEyeCornerWorldPosition(EyeCorner.LeftEyeLeftCorner);
                Vector3 leftEyeRightCorner = GetEyeCornerWorldPosition(EyeCorner.LeftEyeRightCorner);
                Vector3 leftEyeTopCorner = GetEyeCornerWorldPosition(EyeCorner.LeftEyeTopCorner);
                Vector3 leftEyeBottomCorner = GetEyeCornerWorldPosition(EyeCorner.LeftEyeBottomCorner);

                Vector3 rightPupil = GetEyeCornerWorldPosition(EyeCorner.RighEyePupil);
                Vector3 rightEyeLeftCorner = GetEyeCornerWorldPosition(EyeCorner.RightEyeLeftCorner);
                Vector3 rightEyeRightCorner = GetEyeCornerWorldPosition(EyeCorner.RightEyeRightCorner);
                Vector3 rightEyeTopCorner = GetEyeCornerWorldPosition(EyeCorner.RightEyeTopCorner);
                Vector3 rightEyeBottomCorner = GetEyeCornerWorldPosition(EyeCorner.RightEyeBottomCorner);

                //print("eyeLeftCorner: " + leftEyeLeftCorner);
                //print("eyeRightCorner: " + leftEyeRightCorner);
                //print("eyeBottomCorner: " + leftEyeBottomCorner);
                //print("eyeTopCorner: " + leftEyeTopCorner);
                //print("pupil: " + leftPupil);
                float leftEyePositionX = (Mathf.InverseLerp(leftEyeLeftCorner.x + _pupilDecreaseGap.x, leftEyeRightCorner.x - _pupilDecreaseGap.x, leftPupil.x) - 0.5f) * _pupilEffectMultiplier.x;
                float leftEyePositionY = (Mathf.InverseLerp(leftEyeBottomCorner.y + _pupilDecreaseGap.y, leftEyeTopCorner.y - _pupilDecreaseGap.y, leftPupil.y) - 0.45f) * _pupilEffectMultiplier.y;
                //print(leftEyePositionY);

                float rightEyePositionX = (Mathf.InverseLerp(rightEyeLeftCorner.x + _pupilDecreaseGap.x, rightEyeRightCorner.x - _pupilDecreaseGap.x, rightPupil.x) - 0.5f) * _pupilEffectMultiplier.x;
                float rightEyePositionY = (Mathf.InverseLerp(rightEyeBottomCorner.y + _pupilDecreaseGap.y, rightEyeTopCorner.y - _pupilDecreaseGap.y, rightPupil.y) - 0.45f) * _pupilEffectMultiplier.y;

                float useEyePositionX = leftEyePositionX;
                float useEyePositionY = leftEyePositionY;

                //print("Right: " + Mathf.Abs(rightEyeLeftCorner.x - rightEyeRightCorner.x));
                //print("Left: " + Mathf.Abs(leftEyeLeftCorner.x - leftEyeRightCorner.x));

                //if (Mathf.Abs(rightEyeLeftCorner.x - rightEyeRightCorner.x) - Mathf.Abs(leftEyeLeftCorner.x - leftEyeRightCorner.x) > 4)
                //{
                //    useEyePositionX = rightEyePositionX;
                //    useEyePositionY = rightEyePositionY;
                //}

                //print("eyePositionX: " + leftEyePositionX);
                //print("eyePositionY: " + leftEyePositionY);
                //print("frontVector: " + -frontVector.normalized);
                _eyeGaze = (-frontVector + (Vector3.up * useEyePositionY) + (Vector3.right * useEyePositionX)).normalized;
                //print("Normal: " + _eyeGaze);
                EyeCircle.Instance.SetNormal(_eyeGaze);
                //EyeTargetController.Instance.transform.position = leftPupil;
                UnityEngine.Debug.DrawRay(leftPupil, (-frontVector + (Vector3.up * useEyePositionY) + (Vector3.right * useEyePositionX)).normalized * 25, UnityEngine.Color.green);
                //UnityEngine.Debug.DrawRay(leftPupil, ((Vector3.up * useEyePositionY) + (Vector3.right * useEyePositionX)).normalized * 25, UnityEngine.Color.green);

            }

        }


        private void DetectFaceMesh()
        {
            if (_canDetectFace)
            {
                _inputTexture.SetPixels32(_webCamTexture.GetPixels32(_inputPixelData));
                var imageFrame = new ImageFrame(ImageFormat.Types.Format.Srgba, _width, _height, _width * 4, _inputTexture.GetRawTextureData<byte>());
                var currentTimestamp = stopwatch.ElapsedTicks / (System.TimeSpan.TicksPerMillisecond / 1000);
                _graph.AddPacketToInputStream("input_video", new ImageFramePacket(imageFrame, new Timestamp(currentTimestamp))).AssertOk();

                //yield return new WaitForEndOfFrame();

                if (multiFaceLandmarksStream.TryGetNext(out var multiFaceLandmarks))
                {
                    if (multiFaceLandmarks != null && multiFaceLandmarks.Count > 0)
                    {
                        _multiFaceLandmarksAnnotationController.DrawNow(multiFaceLandmarks);

                        int faceLandmarkIndex = 0;
                        int leftEyeLandmarkIndex = 0;
                        int rightEyeLandmarkIndex = 0;

                        for (int i = 0; i < multiFaceLandmarks[0].Landmark.Count; i++)
                        {
                            //if (_faceLandmarksIndexes.Contains(i))
                            if (_faceCircles.Count > i)
                            {
                                Vector3 vector3;
                                //Image Point
                                Vector3 point = screenRect.GetPoint(multiFaceLandmarks[0].Landmark[i]);

                                SetCircleProperty(_faceCircles[faceLandmarkIndex], point, out vector3, i.ToString());
                                FaceLandmark landmark = new FaceLandmark(multiFaceLandmarks[0].Landmark[i], new Point3f(vector3.x, vector3.y, vector3.z), new Point2f(point.x, point.y));

                                _calculatedFaceLandmarks.Add(landmark);

                                faceLandmarkIndex++;
                            }

                            if (_leftEyeLandmarksIndexes.Contains(i))
                            {
                                //float x = multiFaceLandmarks[0].Landmark[i].X * _width;
                                //float y = multiFaceLandmarks[0].Landmark[i].Y * _height;
                                //Vector2 vector2 = new Vector2(x, y);

                                //ObjectPoint
                                Vector3 vector3;
                                //Image Point
                                Vector3 point = screenRect.GetPoint(multiFaceLandmarks[0].Landmark[i]);

                                SetCircleProperty(_leftEyeCircles[leftEyeLandmarkIndex], point, out vector3,i.ToString());

                                EyePoint eyePoint = _leftEyePoints.Find((x) => x.Index == i);
                                eyePoint.ImagePosition = point;
                                eyePoint.WorldPosition = vector3;
                                eyePoint.GameObject = _leftEyeCircles[leftEyeLandmarkIndex];

                                //FaceLandmark landmark = new FaceLandmark(multiFaceLandmarks[0].Landmark[i],new Point3f(vector3.x, vector3.y, vector3.z), new Point2f(point.x, point.y));
                                leftEyeLandmarkIndex++;
                            }

                            if (_rightEyeLandmarksIndexes.Contains(i))
                            {
                                //float x = multiFaceLandmarks[0].Landmark[i].X * _width;
                                //float y = multiFaceLandmarks[0].Landmark[i].Y * _height;
                                //Vector2 vector2 = new Vector2(x, y);

                                //ObjectPoint
                                Vector3 vector3;
                                //Image Point
                                Vector3 point = screenRect.GetPoint(multiFaceLandmarks[0].Landmark[i]);

                                SetCircleProperty(_rightEyeCircles[rightEyeLandmarkIndex], point, out vector3, i.ToString());

                                EyePoint eyePoint = _rightEyePoints.Find((x) => x.Index == i);
                                eyePoint.ImagePosition = point;
                                eyePoint.WorldPosition = vector3;
                                eyePoint.GameObject = _rightEyeCircles[rightEyeLandmarkIndex];

                                //FaceLandmark landmark = new FaceLandmark(multiFaceLandmarks[0].Landmark[i], new Point3f(vector3.x, vector3.y, vector3.z), new Point2f(point.x, point.y));
                                rightEyeLandmarkIndex++;
                            }

                        }

                        #region Head Direction
                        float normalizedFocaleY = 1.28f; // Logitech 922
                        float focalLength = _height * normalizedFocaleY;
                        float cx = _width / 2;
                        float cy = _height / 2;

                        // Kameranýn iç parametrelerini ve distorsiyon katsayýlarýný ayarla
                        Mat cameraMatrix = new Mat(3, 3, MatType.CV_64FC1);
                        cameraMatrix.Set<double>(0, 0, focalLength);
                        cameraMatrix.Set<double>(1, 1, focalLength);
                        cameraMatrix.Set<double>(0, 2, cx);
                        cameraMatrix.Set<double>(1, 2, cy);
                        cameraMatrix.Set<double>(2, 2, 1.0);

                        Mat distCoeffs = new Mat(1, 5, MatType.CV_64FC1);

                        MatOfPoint2f imagePoints = MatOfPoint2f.FromArray(new Point2f[]
                        {
                            _calculatedFaceLandmarks[0].ImagePosition ,      // Nose
                            _calculatedFaceLandmarks[1].ImagePosition,  // Left Eye Corner
                            _calculatedFaceLandmarks[2].ImagePosition,  // Right Eye Corner
                            _calculatedFaceLandmarks[3].ImagePosition,  // Left Mouth Corner
                            _calculatedFaceLandmarks[4].ImagePosition,  // Right Mouth Corner
                            _calculatedFaceLandmarks[5].ImagePosition,  // Chin
                        });

                        MatOfPoint3f objectPoints = MatOfPoint3f.FromArray(new Point3f[] {
                            _calculatedFaceLandmarks[0].WorldPosition ,      // Nose
                            _calculatedFaceLandmarks[1].WorldPosition,  // Left Eye Corner
                            _calculatedFaceLandmarks[2].WorldPosition,  // Right Eye Corner
                            _calculatedFaceLandmarks[3].WorldPosition,  // Left Mouth Corner
                            _calculatedFaceLandmarks[4].WorldPosition,  // Right Mouth Corner
                            _calculatedFaceLandmarks[5].WorldPosition,  // Chin
                        });

                        // solvePnP fonksiyonunu kullanarak rotasyon ve çeviri vektörlerini bul
                        Mat rvec = new Mat();
                        Mat tvec = new Mat();
                        Cv2.SolvePnP(objectPoints, imagePoints, cameraMatrix, distCoeffs, rvec, tvec);

                        // Rodrigues fonksiyonunu kullanarak rotasyon matrisini elde et
                        Mat rotationMatrix = new Mat();
                        Cv2.Rodrigues(rvec, rotationMatrix);

                        // Yüzün ön tarafýný temsil eden bir vektör oluþtur

                        // Rotasyon matrisi ile ön taraf vektörünü dönüþtür
                        MatOfPoint3f rotatedFrontVectorMat = new MatOfPoint3f();
                        MatOfPoint3f frontVectorMat = MatOfPoint3f.FromArray(new Point3f[] { new Point3f(0, 0, 1) });
                        Cv2.Transform(frontVectorMat, rotatedFrontVectorMat, rotationMatrix);

                        // Get the rotated front vector as a Vector3
                        Vector3 rotatedFrontVector = new Vector3(
                            (float)rotatedFrontVectorMat.At<Point3f>(0, 0).X,
                            (float)rotatedFrontVectorMat.At<Point3f>(0, 0).Y,
                            (float)rotatedFrontVectorMat.At<Point3f>(0, 0).Z
                        );

                        //Now, rotatedFrontVector contains the rotated front vector
                        frontVector = rotatedFrontVector;
                        #endregion


                        //frontVector = Vector3.forward;
                    }
                }
                else
                {
                    _multiFaceLandmarksAnnotationController.DrawNow(null);
                }
            }
        }

        private void OnDestroy()
        {
            if (_webCamTexture != null)
            {
                _webCamTexture.Stop();
            }

            if (_graph != null)
            {
                try
                {
                    _graph.CloseInputStream("input_video").AssertOk();
                    _graph.WaitUntilDone().AssertOk();
                }
                finally
                {

                    _graph.Dispose();
                }
            }
        }

        private Vector3 GetEyeCornerWorldPosition(EyeCorner eyeCorner)
        {
            for (int i = 0; i < _leftEyePoints.Count; i++)
            {
                if (_leftEyePoints[i].Index == (int)eyeCorner)
                    return _leftEyePoints[i].WorldPosition;
            }

            for (int i = 0; i < _rightEyePoints.Count; i++)
            {
                if (_rightEyePoints[i].Index == (int)eyeCorner)
                    return _rightEyePoints[i].WorldPosition;
            }

            return Vector3.zero;
        }


        private void SetCircleProperty(GameObject circle,Vector3 point,out Vector3 worldPosition,string name = "")
        {
            circle.GetComponent<RectTransform>().anchoredPosition = point;
            circle.GetComponentInChildren<TextMeshProUGUI>().text = (name == "") ? "Circle" : name;
            circle.name = name;
            circle.GetComponent<Image>().color = UnityEngine.Color.blue;

            worldPosition = circle.GetComponent<RectTransform>().TransformPoint(circle.GetComponent<RectTransform>().anchoredPosition);
            //ObjectPoints
        }

        private Vector3 GetPointWorldPosition(Vector3 point)
        {
            return EyeTargetController.Instance.GetComponent<RectTransform>().TransformPoint(EyeTargetController.Instance.GetComponent<RectTransform>().anchoredPosition);
        }


        [Serializable]
        public struct FaceLandmark
        {
            public NormalizedLandmark Landmark;
            public Point3f WorldPosition;
            public Point2f ImagePosition;

            public FaceLandmark(NormalizedLandmark normalizedLandmark,Point3f worldPosition, Point2f imagePosition)
            {
                WorldPosition = worldPosition;
                ImagePosition = imagePosition;
                Landmark = normalizedLandmark;
            }
        }

    }
}