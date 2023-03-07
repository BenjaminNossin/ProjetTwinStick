using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerAction
{
    public bool IsInAction { get; }
    public void MakeAction();
    public void SetupAction(params object[] arguments);
    public void DeactivateAction();
    public void ActivateAction();
    public event System.Action MakeActionEvent;
}
