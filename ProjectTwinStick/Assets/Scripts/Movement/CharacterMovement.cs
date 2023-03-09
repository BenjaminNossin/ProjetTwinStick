using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private PlayerStats _stats;
    [SerializeField] GameplayTag MovementBlocker;
    [SerializeField] GameplayTag AimSlower;

    private CharacterController _characterController;
    private GameplayTagContainer _tagContainer;
    private Vector3 CurrentVelocity;

    private void Awake()
    {
        Debug.Log("Character awake");
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
        Debug.Log("Character teleport");
        _characterController.transform.position = newPosition;
    }
    
    public void UpdateRotation(Vector3 direction)
    {
        if (_tagContainer.HasTag(AimSlower))
        {
            Vector3 forward = transform.forward;
            forward = Vector3.RotateTowards(forward, direction, _stats.RestrictedAimSpeed * Time.deltaTime, 1f);
            transform.forward = forward;
        }
        else transform.forward = direction;
    }

    public void SetVelocity(Vector3 velocity)
    {
        CurrentVelocity = velocity;
    }
}