using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBehavior : MonoBehaviour
{

    [SerializeField] CinemachineMixingCamera cameraMix;
    [SerializeField]float min = 0f, max = 10f;
    [SerializeField] float mixValueUp;
    [SerializeField]float mixValueDownLeft;
    [SerializeField] float mixValueUpLeft;
    [SerializeField] float mixValueUpRight;
    [SerializeField]float mixValueDownRight;
    void Update()
    {
        CamUp();
        CamDownLeft();
        CamDownRight();
        CamUpLeft();
        CamUpRight();

        clamp();
    }
    void clamp()
    {
        mixValueUp = Mathf.Clamp(mixValueUp, min, max);
        mixValueDownLeft = Mathf.Clamp(mixValueDownLeft, min, max);
        mixValueDownRight = Mathf.Clamp(mixValueDownRight, min, max);
        mixValueUpLeft = Mathf.Clamp(mixValueUpLeft, min, max);
        mixValueUpRight = Mathf.Clamp(mixValueUpRight, min, max);
    }
    void CamUp()
    {
        cameraMix.m_Weight0 = mixValueUp;
    }

    void CamDownLeft()
    {
        cameraMix.m_Weight1 = mixValueDownLeft;
    }  
    void CamUpLeft()
    {
        cameraMix.m_Weight2 = mixValueUpLeft;
    }
    void CamDownRight()
    {
        cameraMix.m_Weight3 = mixValueDownRight;
    }
    void CamUpRight()
    {
        cameraMix.m_Weight4 = mixValueUpRight;
    } 

}
