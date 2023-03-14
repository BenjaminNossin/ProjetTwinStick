using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour, IPlayerAction
{
    public bool IsInAction
    {
        get => _isInMoving;
    }

 

    public CharacterMovement characterMovement;

 
    //TODO : move movement stats to dedicated stat system
    [SerializeField] public PlayerStats stats;
    private float CurrentAcceleration;

    public Vector2 CurrentMovementInputs;
    private bool _isInMoving;

    public event Action OnMove;
    public event Action OnCancelMove;

    public void PerformAction(params object[] arguments)
    {
        CurrentMovementInputs = (Vector2)arguments[0];
    }

    public void CancelAction(params object[] arguments)
    {
    }

    public void SetupAction(params object[] arguments)
    {
    }

    public void DisableAction()
    {
        CurrentMovementInputs = Vector2.zero;
        characterMovement.SetVelocity(Vector3.zero);
        OnCancelMove?.Invoke();
    }

    public void EnableAction()
    {
    }


    private void Update()
    {
       
        UpdateVelocity();
    }

    private void UpdateVelocity()
    {
        if (CurrentMovementInputs.magnitude > stats.MovementInputThreshold)
        {
            CurrentAcceleration = Mathf.MoveTowards(CurrentAcceleration, 1, (Time.deltaTime / stats.AccelerationTime));
            OnMove?.Invoke();
            _isInMoving = true;
        }
        else
        {
            CurrentAcceleration = Mathf.MoveTowards(CurrentAcceleration, 0, (Time.deltaTime / stats.DecelerationTime));
            OnCancelMove?.Invoke();
            _isInMoving = false;
        }

        characterMovement.SetVelocity(new Vector3(CurrentMovementInputs.x, 0, CurrentMovementInputs.y) *
                                      (stats.Speed * CurrentAcceleration));
    }

    public void SetControllerSpawnPosition(Vector3 _pos)
    {
        characterMovement.TeleportPlayer(_pos);
    }
}