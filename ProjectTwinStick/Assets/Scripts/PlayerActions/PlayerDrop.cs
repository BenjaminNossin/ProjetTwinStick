using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrop : MonoBehaviour, IPlayerAction
{
    public bool IsInAction { get; }
    public void MakeAction()
    {

    }

    public void PerformAction(params object[] arguments)
    {
      
    }

    public void CancelAction(params object[] arguments)
    {
        
    }

    public void SetupAction(params object[] arguments)
    {
       
    }

    public void DisableAction()
    {
       
    }

    public void EnableAction()
    {
    }

    public event Action PerformActionEvent;
}
