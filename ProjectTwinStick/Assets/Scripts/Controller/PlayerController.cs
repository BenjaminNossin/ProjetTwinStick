using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour, IController
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerShoot _playerShoot;
    [SerializeField] private PlayerTake _playerTake;
    [SerializeField] private PlayerDrop _playerDrop;
    [SerializeField] private PlayerThrow _playerThrow;
    private bool isActive = false;

    [SerializeField] GameObject playerObject;

    private CharacterMovement _characterMovement;
    private PlayerInput _playerInput;

    private void OnEnable()
    {
        _characterMovement = playerObject.GetComponent<CharacterMovement>();
        _playerInput = GetComponent<PlayerInput>();
        InputAction movement = _playerInput.currentActionMap["Movement"];
        Debug.Log(movement);
        movement.performed += UpdateMovementInput;
        movement.canceled += UpdateMovementInput;
    }

    private void OnDisable()
    {
        InputAction movement = _playerInput.currentActionMap["Movement"];
        movement.performed -= UpdateMovementInput;
        movement.canceled -= UpdateMovementInput;
    }

    private void UpdateMovementInput(InputAction.CallbackContext context)
    {
        if (playerObject != null && isActive)
        {
            Vector2 inputs = context.ReadValue<Vector2>();
            Debug.Log("Updated movement inputs : " + inputs);
            _characterMovement.UpdateMovementInputs(inputs);
        }
    }

    [FormerlySerializedAs("_playerMovement")] [SerializeField]
    private SamplePlayerMovement samplePlayerMovement;

    void Start()
    {
        ActivateController();
        _playerMovement.SetupAction();
        _playerShoot.SetupAction();
        _playerTake.SetupAction();
        _playerDrop.SetupAction();
        _playerThrow.SetupAction();
    }

    public void ActivateController()
    {
        isActive = true;
        _playerDrop.ActivateAction();
        _playerThrow.ActivateAction();
        _playerShoot.ActivateAction();
        _playerTake.ActivateAction();
        _playerMovement.ActivateAction();
    }

    public void DeactivateController()
    {
        isActive = false;
        if (playerObject != null)
        {
            _characterMovement.UpdateMovementInputs(Vector2.zero);
        }
        _playerDrop.DeactivateAction();
        _playerThrow.DeactivateAction();
        _playerShoot.DeactivateAction();
        _playerTake.DeactivateAction();
        _playerMovement.DeactivateAction();
    }
}