using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FPSCamSetter : MonoBehaviour
{
    private void Awake()
    {
        CinemachineVirtualCamera temp = GetComponent<CinemachineVirtualCamera>();
        GameObject look = GameObject.Find("Look");

        temp.Follow = look.transform;
        temp.LookAt = look.transform;
    }
}
