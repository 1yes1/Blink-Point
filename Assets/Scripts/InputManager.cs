using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlinkPoints
{
    public static class InputManager
    {
        public static bool IsKeyPressed => Input.GetKeyDown(KeyCode.Space);

        public static bool IsExitKeyPressed => Input.GetKeyDown(KeyCode.Escape);
    }
}

