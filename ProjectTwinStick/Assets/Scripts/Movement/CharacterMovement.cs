using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{ 
    //TODO : move movement stats to dedicated stat system
    [SerializeField] float speed;
    [SerializeField] float acceleration;
    [SerializeField, Range(0.0001f,1f)] float InputThreshold = 0.1f;

    private CharacterController _characterController;

    private Vector2 CurrentMovementInputs = Vector2.zero;
    private Vector3 CurrentVelocity;
    private float CurrentAcceleration = 0f;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        UpdateVelocity();
        _characterController.Move(CurrentVelocity);
    }

    private void UpdateVelocity()
    {
        if (CurrentMovementInputs.magnitude < InputThreshold)
        {
            CurrentAcceleration = Mathf.MoveTowards(CurrentAcceleration, 1, acceleration * Time.deltaTime);
        }
        else
        {
            CurrentAcceleration = Mathf.MoveTowards(CurrentAcceleration, 0, acceleration * Time.deltaTime);
        }

        CurrentVelocity = new Vector3(CurrentMovementInputs.x,0, CurrentMovementInputs.y) * (speed * CurrentAcceleration);
    }

    public void UpdateMovementInputs(Vector2 NewInputs)
    {
        CurrentMovementInputs = NewInputs;
    }
    
    
}
