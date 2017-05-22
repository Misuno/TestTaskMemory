using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Misuno.TestTaskMemory
{
    public class TimerController : MonoBehaviour
    {
        private float passedTime;
        private float startTime;
        private Image myImage;


        void Awake()
        {
            myImage = GetComponent<Image>();
        }

        void Start()
        {
            if (World.timer > 0f)
            {
                startTime = Time.time;
                myImage.fillAmount = 1f;
                StartCoroutine(Timer());
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private IEnumerator Timer()
        {
            while (true)
            {
                float passedTime = Time.time - startTime;
                if (passedTime >= World.timer)
                {
                    SceneManager.LoadScene("Menu");
                    yield break;
                }

                float amount = 1f - passedTime / World.timer;
                myImage.fillAmount = amount;

                yield return null;
            }
        }
    }
}
