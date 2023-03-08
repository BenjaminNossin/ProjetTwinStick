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

    private PlayerInput _playerInput;

    private void OnEnable()
    {
        _playerInput = GetComponent<PlayerInput>();
        InputAction movement = _playerInput.currentActionMap["Movement"];
        movement.performed += UpdateMovementInput;
        movement.canceled += UpdateMovementInput;
        
        InputAction take = _playerInput.currentActionMap["Take"];
        take.performed += TryTakeInput;
        
        InputAction shoot = _playerInput.currentActionMap["Shoot"];
        shoot.performed += UpdateShootInput;
        shoot.canceled += UpdateShootInput;
        
        InputAction throwAction = _playerInput.currentActionMap["Throw"];
        throwAction.performed += StartAiming;
        throwAction.canceled += TryThrow;
        
        InputAction cancelThrow = _playerInput.currentActionMap["CancelThrow"];
        cancelThrow.performed += CancelThrow;
    }

    // NULLREFERENCEEXCEPTION
    /* private void OnDisable()
    {
        InputAction movement = _playerInput.currentActionMap["Movement"];
        if (movement != null)
        {
            movement.performed -= UpdateMovementInput;
            movement.canceled -= UpdateMovementInput;
        }

        InputAction take = _playerInput.currentActionMap["Take"];
        if (take != null)
        {
            take.performed -= TryTakeInput;
        }

        InputAction shoot = _playerInput.currentActionMap["Shoot"];
        if (shoot != null)
        {
            shoot.performed += UpdateShootInput;
            shoot.canceled += UpdateShootInput;
        }
    } */

    #region InputCallbacks

    private void Start()
    {
        Debug.Log("Spawning new player");
        AddSelfToCurrentState();
    }

    private void AddSelfToCurrentState()
    {
        if (GameManager.Instance != null)
        {
            currentState = GameManager.Instance.GetCurrentState();
        }

        currentState.AddPlayerController(this);
    }

    private void StartAiming(InputAction.CallbackContext context)
    {
        _playerThrow.PerformAction(false);
    }

    private void TryThrow(InputAction.CallbackContext context)
    {
        _playerThrow.PerformAction(true);
    }

    private void CancelThrow(InputAction.CallbackContext obj)
    {
        _playerThrow.CancelAction();
    }

    private void UpdateShootInput(InputAction.CallbackContext context)
    {
        if (playerObject != null && isActive)
        {
            Vector2 inputs = context.ReadValue<Vector2>();
            _playerShoot.PerformAction(inputs);
        }
    }

    private void UpdateMovementInput(InputAction.CallbackContext context)
    {
        if (playerObject != null && isActive)
        {
            Vector2 inputs = context.ReadValue<Vector2>();
            _playerMovement.PerformAction(inputs);
        }
    }

    private void TryTakeInput(InputAction.CallbackContext context)
    {
        Debug.Log("Take input");
        _playerTake.PerformAction();
    }

    #endregion

    private State currentState;

    public void SetUpController()
    {
        _playerMovement.SetupAction();
        _playerShoot.SetupAction();
        _playerTake.SetupAction();
        _playerDrop.SetupAction();
        _playerThrow.SetupAction();
    }

    public void ActivateController()
    {
        isActive = true;
        _playerDrop.EnableAction();
        _playerThrow.EnableAction();
        _playerShoot.EnableAction();
        _playerTake.EnableAction();
        _playerMovement.EnableAction();

    }

    public void DeactivateController()
    {
        isActive = false;
        _playerDrop.DisableAction();
        _playerThrow.DisableAction();
        _playerShoot.DisableAction();
        _playerTake.DisableAction();
        _playerMovement.DisableAction();

        // currentState.RemovePlayerController(this);
    }
}