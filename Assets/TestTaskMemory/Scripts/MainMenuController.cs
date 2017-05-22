using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Misuno.TestTaskMemory
{
    public class MainMenuController : MonoBehaviour
    {
        public void PlayButtonPressed()
        {
            SceneManager.LoadScene("Game");
        }        
    }
}