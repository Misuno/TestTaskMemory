using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Misuno.TestTaskMemory
{
    public class MainMenuController : MonoBehaviour
    {
        [Range(0f, 600f)]
        public float timerValue = 120f;

        public void PlayButtonPressed()
        {
            SceneManager.LoadScene("Game");
        }

        public void ToggleTimer(bool toggled)
        {
            World.timer = toggled ? timerValue : 0f;
        }
    }
}