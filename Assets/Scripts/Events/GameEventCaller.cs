using UnityEngine;
using System.Collections;
using System;
using System.Drawing;

namespace BlinkPoints
{
    public class GameEventCaller: IGameEvents
    {
        private GameEventReceiver _gameEventReceiver;

        public static GameEventCaller Instance
        {
            get
            {
                return GameManager.Instance.GameEventCaller;
            }
        }

        public GameEventCaller(GameEventReceiver gameEventReceiver)
        {
            _gameEventReceiver = gameEventReceiver;
        }

        public void OnPointVisible(Point point)
        {
            _gameEventReceiver.OnPointVisible(point);
        }

        public void OnKeyPressed()
        {
            _gameEventReceiver.OnKeyPressed();
        }

        public void OnPointInvisible(Point point)
        {
            _gameEventReceiver.OnPointInvisible(point);
        }

        public void OnCompleted()
        {
            _gameEventReceiver.OnCompleted();
        }

        public void OnTestStarted()
        {
            _gameEventReceiver.OnTestStarted();
        }
    }

}
