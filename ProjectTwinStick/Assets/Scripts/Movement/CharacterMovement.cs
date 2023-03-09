using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] GameplayTag MovementBlocker;
    
    private CharacterController _characterController;
    private GameplayTagContainer _tagContainer;
    private Vector3 CurrentVelocity;
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _tagContainer = GetComponent<GameplayTagContainer>();
    }

    private void Update()
    {
        if (_tagContainer.HasTag(MovementBlocker))
        {
            return;
        }
        UpdateVelocity();
        _characterController.Move(CurrentVelocity * Time.deltaTime);
    }

    private void UpdateVelocity()
    {
        //CurrentVelocity.y = -1;
    }

    public void TeleportPlayer(Vector3 newPosition)
    {
        _characterController.transform.position = newPosition;
    }

   public void SetVelocity(Vector3 velocity)
   {
        CurrentVelocity = velocity;
    }
}
