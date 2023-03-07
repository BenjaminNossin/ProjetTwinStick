using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrow : MonoBehaviour, IPlayerAction
{
    public bool IsInAction { get; }
    public void MakeAction()
    {
 
    }

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
