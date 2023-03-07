using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour, IPlayerAction
{
    public bool IsInAction { get; }
    public void MakeAction()
    {
        throw new NotImplementedException();
    }

    public void SetupAction(params object[] arguments)
    {
        throw new NotImplementedException();
    }

    public void DeactivateAction()
    {
        throw new NotImplementedException();
    }

    public void ActivateAction()
    {
        throw new NotImplementedException();
    }



    public event Action MakeActionEvent;
}
