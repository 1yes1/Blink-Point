using UnityEngine;
using System.Collections;
using System;
using System.Drawing;

namespace BlinkPoints
{
    public class GameEventCaller: IGameEvents
    {
        private GameEventReceiver _gameEventReceiver;

        public static GameEventCaller Instance => GameManager.Instance.GameEventCaller;

        public GameEventCaller(GameEventReceiver gameEventReceiver) => _gameEventReceiver = gameEventReceiver;

        public void OnPointVisible(Point point) => _gameEventReceiver.OnPointVisible(point);

        public void OnKeyPressed() => _gameEventReceiver.OnKeyPressed();

        public void OnPointInvisible(Point point) => _gameEventReceiver.OnPointInvisible(point);

        public void OnCompleted() => _gameEventReceiver.OnCompleted();

        public void OnTestStarted() => _gameEventReceiver.OnTestStarted();

        public void OnBeforePointInvisible(Point point) => _gameEventReceiver.OnBeforePointInvisible(point);

        public void OnCountdownEnded() => _gameEventReceiver.OnCountdownEnded();
    }

}
