using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BlinkPoints
{
    public class SceneController : MonoBehaviour
    {

        public void OpenTestScene()
        {
            SceneManager.LoadScene(1);
        }

        public void Exit()
        {
            Application.Quit(); 
        }

        public void OpenScene(int sceneIndex = 2)
        {
            SceneManager.LoadScene(sceneIndex);
        }

    }

}
