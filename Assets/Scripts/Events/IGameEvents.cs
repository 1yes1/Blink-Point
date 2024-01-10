using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlinkPoints
{
    public interface IGameEvents
    {
        public void OnCountdownEnded();
        public void OnTestStarted();
        public void OnPointVisible(Point point);
        public void OnPointInvisible(Point point);
        public void OnKeyPressed();
        public void OnCompleted();
        public void OnBeforePointInvisible(Point point);

    }

}
