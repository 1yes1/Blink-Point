using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlinkPoints
{
    public class Controller : MonoBehaviour
    {
        void Start()
        {

        }

        void Update()
        {
            if (InputManager.IsKeyPressed)
            {
                GameEventCaller.Instance.OnKeyPressed();
            }
        }
    }
}

