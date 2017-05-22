using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Misuno.TestTaskMemory
{
    public class ToggleChecker : MonoBehaviour
    {
        void Start()
        {
            GetComponent<Toggle>().isOn = World.timer > 0f;
        }
    }
}