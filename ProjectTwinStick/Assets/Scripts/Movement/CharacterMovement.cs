using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private PlayerStats _stats;
    [SerializeField] private SlowManager _slowManager;
    [SerializeField] GameplayTag MovementBlocker;
    [SerializeField] GameplayTag AimSlower;
    [SerializeField] GameplayTag BlockAim;

    private CharacterController _characterController;
    private GameplayTagContainer _tagContainer;
    private Vector3 CurrentVelocity;

    public event Action<float> OnSetVelocity;


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
            CurrentVelocity = Vector3.zero;
        }

        UpdateVelocity();
        _characterController.Move(CurrentVelocity * (Time.deltaTime * _slowManager.GetCurrentSlowMultiplier()));
    }

    private void UpdateVelocity()
    {
        CurrentVelocity.y = -1;
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
        else if (!_tagContainer.HasTag(BlockAim))
        {
            transform.forward = direction;
        }
    }

    public void SetVelocity(Vector3 velocity)
    {
        CurrentVelocity = velocity;
        OnSetVelocity?.Invoke(velocity.magnitude);
        
    }
}