using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAim : MonoBehaviour, IPlayerAction
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private CharacterMovement _characterMovement;
    
    public bool IsInAction { get; }


    private bool UpdateAim = false;
    private Vector3 target = Vector3.forward;

    private void Update()
    {
        if (UpdateAim)
        {
            _characterMovement.UpdateRotation(target);
        }
    }

    public void MakeAction()
    {

    }

    public void PerformAction(params object[] arguments)
    {
        Vector2 inputs = (Vector2)arguments[0];
        if (inputs.magnitude > _playerStats.AimInputThreshold)
        {
            target = new Vector3(inputs.x, 0, inputs.y);
            UpdateAim = true;
        }
        else
        {
            UpdateAim = false;
        }
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

    public UnityEvent PerformEvent { get; }
    public UnityEvent CancelEvent { get; }

    public event Action PerformActionEvent;
}
