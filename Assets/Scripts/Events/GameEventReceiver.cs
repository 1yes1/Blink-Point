using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BlinkPoints
{
    public class GameEventReceiver : IGameEvents
    {
        public static event Action<Point> OnPointVisibleEvent;

        public static event Action<Point> OnPointInvisibleEvent;

        public static event Action OnKeyDownEvent;

        public static event Action OnCompletedEvent;


        public void OnKeyPressed()
        {
            OnKeyDownEvent?.Invoke();
        }

        public void OnPointInvisible(Point point)
        {
            OnPointInvisibleEvent?.Invoke(point);
        }

        public void OnPointVisible(Point point)
        {
            OnPointVisibleEvent?.Invoke(point);
        }

        public void OnCompleted()
        {
            OnCompletedEvent?.Invoke();
        }
    }
}
