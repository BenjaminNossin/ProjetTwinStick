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
        
    }

    private void Start()
    {
        SetUpController(); 
    }

    private void UpdateMovementInput(InputAction.CallbackContext context)
    {
        if (playerObject != null && isActive)
        {
            Vector2 inputs = context.ReadValue<Vector2>();
            _playerMovement.MakeAction(inputs);
        }
    }

    private void TryTakeInput(InputAction.CallbackContext context)
    {
        Debug.Log("Take input");
        _playerTake.MakeAction();
    }

    [FormerlySerializedAs("_playerMovement")] [SerializeField]
    private SamplePlayerMovement samplePlayerMovement;
    
    private State currentState; 

    private void SetUpController()
    {
        if (GameStates.Instance != null)
        {
            currentState = GameStates.Instance.currentContext.GetCurrentState();
        }

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

        currentState.AddPlayerController(this);
    }

    public void DeactivateController()
    {
        isActive = false;
        _playerDrop.DeactivateAction();
        _playerThrow.DeactivateAction();
        _playerShoot.DeactivateAction();
        _playerTake.DeactivateAction();
        _playerMovement.DeactivateAction();

        currentState.RemovePlayerController(this);
    }
}