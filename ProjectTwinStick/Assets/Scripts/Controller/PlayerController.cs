using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour, IController
{
    private bool isActive = false;

    [SerializeField] GameObject playerObject;

    private CharacterMovement _playerMovement;
    private PlayerInput _playerInput;
    
    private void OnEnable()
    {
	    _playerMovement = playerObject.GetComponent<CharacterMovement>();
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

    private void Start()
    {
	    ActivateController();
    }

    private void UpdateMovementInput(InputAction.CallbackContext context)
    {
	    if (playerObject != null && isActive)
	    {
		    Vector2 inputs = context.ReadValue<Vector2>();
		    Debug.Log("Updated movement inputs : " + inputs);
		    _playerMovement.UpdateMovementInputs(inputs);
	    }
    }
    
    public void ActivateController()
    {
        isActive = true;
    }

	public void DeactivateController(){
		isActive = false;
		if (playerObject != null)
		{
			_playerMovement.UpdateMovementInputs(Vector2.zero);
		}
	}

}
