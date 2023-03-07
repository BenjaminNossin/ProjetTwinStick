using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IPlayerAction
{
    public bool IsInAction { get => _isInMoving; }
    private bool _canMoving = true;

    public CharacterMovement characterMovement;
    public event Action MakeActionEvent;
    
    //TODO : move movement stats to dedicated stat system
    [SerializeField] float speed;
   [SerializeField] float accelerationTime;
     [SerializeField] private float decelerationTime = .1f;
    [SerializeField, Range(0.0001f,1f)] float InputThreshold = 0.1f;
    private float CurrentAcceleration;
    
    public Vector2 CurrentMovementInputs;
    private bool _isInMoving;

    public void MakeAction(params object[] arguments)
    {
        CurrentMovementInputs =(Vector2) arguments[0];
    }

    public void SetupAction(params object[] arguments)
    {
        
    }

    public void DeactivateAction()
    {
        CurrentMovementInputs = Vector2.zero;
        characterMovement.SetVelocity( Vector3.zero);
    }
    public void ActivateAction()
    {

    }
    
    private void Update()
    {
        if(!_canMoving) return;
        UpdateVelocity();
    }

    private void UpdateVelocity()
    {
        if (CurrentMovementInputs.magnitude > InputThreshold)
        {
            CurrentAcceleration = Mathf.MoveTowards(CurrentAcceleration, 1, (Time.deltaTime / accelerationTime));
            _isInMoving = true; 
        }
        else
        {
            CurrentAcceleration = Mathf.MoveTowards(CurrentAcceleration, 0, (Time.deltaTime / decelerationTime));
            _isInMoving = false;
        }
        characterMovement.SetVelocity( new Vector3(CurrentMovementInputs.x,0, CurrentMovementInputs.y) * (speed * CurrentAcceleration));
    }
}
