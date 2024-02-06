using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class CameraSens : MonoBehaviour
{
    Slider settingsSlider;

    private void Awake()
    {
        settingsSlider = GetComponent<Slider>();

        if (settingsSlider != null)
        {
            CinemachineInputProvider.sensitivity = settingsSlider.value;
        }
    }

    public void UpdateSensitivity()
    {
        CinemachineInputProvider.sensitivity = settingsSlider.value;
    }
}

