using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BlinkPoints
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;

        private List<Point> _mapPoints = new List<Point>();

        private Point currentPoint;

        public static GameManager Instance => _instance;
        public GameEventCaller GameEventCaller { get; private set; }
        public GameEventReceiver GameEventReceiver { get; private set; }

        private void OnEnable()
        {
            GameEventReceiver.OnPointVisibleEvent += OnPointVisible;
            GameEventReceiver.OnPointInvisibleEvent += OnPointInvisible;
            GameEventReceiver.OnKeyDownEvent += CheckCurrentPoint;
            GameEventReceiver.OnCompletedEvent += OnCompleted;
        }


        private void OnDisable()
        {
            GameEventReceiver.OnPointVisibleEvent -= OnPointVisible;
            GameEventReceiver.OnKeyDownEvent -= CheckCurrentPoint;
            GameEventReceiver.OnPointInvisibleEvent -= OnPointInvisible;
            GameEventReceiver.OnCompletedEvent -= OnCompleted;
        }

        private void Awake()
        {
            if(_instance == null)
                _instance = this;

            InitializeEvents();
        }

        private void InitializeEvents()
        {
            GameEventReceiver = new GameEventReceiver();
            GameEventCaller = new GameEventCaller(GameEventReceiver);
        }

        void Update()
        {
            if (InputManager.IsExitKeyPressed)
            {
                GoToMainMenu();
            }
        }

        public void GoToMainMenu()
        {
            SceneManager.LoadScene(0);
        }

        private void OnPointVisible(Point point)
        {
            currentPoint = point;
        }

        private void OnPointInvisible(Point point)
        {
            currentPoint = null;
        }

        private void CheckCurrentPoint()
        {
            if (currentPoint != null)
            {
                _mapPoints.Add(currentPoint);
                //print("Added point");
            }
            //else
            //{
            //    //print("NullPoint");
            //}
        }

        private void OnCompleted()
        {
            print("Completed");
            for (int i = 0; i < _mapPoints.Count; i++)
            {
                _mapPoints[i].Show();
            }
        }

    }

}
