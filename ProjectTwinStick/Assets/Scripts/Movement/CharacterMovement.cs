using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    private CharacterController _characterController;
    private Vector3 CurrentVelocity;
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        UpdateVelocity();
        _characterController.Move(CurrentVelocity * Time.deltaTime);
    }

    private void UpdateVelocity()
    {
      
    }

   public void SetVelocity(Vector3 velocity)
   {
        CurrentVelocity = velocity ;
    }
}
