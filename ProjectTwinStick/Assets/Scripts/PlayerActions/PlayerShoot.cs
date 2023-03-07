using System;
using System.Collections;
using System.Collections.Generic;
using HelperPSR.Pool;
using UnityEngine;

public class PlayerShoot : MonoBehaviour, IPlayerAction
{
    public bool IsInAction { get; }





    public void MakeAction(params object[] arguments)
    {
     
    }

    public void SetupAction(params object[] arguments)
    {
       
    }

    public void DeactivateAction()
    {
      
    }

    public void ActivateAction()
    {
     
    }



    public event Action MakeActionEvent;
}
