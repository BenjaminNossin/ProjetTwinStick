using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour, IController
{
	[SerializeField] private PlayerMovement _playerMovement;
	[SerializeField] private PlayerShoot _playerShoot;
	[SerializeField] private PlayerTake _playerTake;
	[SerializeField] private PlayerDrop _playerDrop;
	[SerializeField] private PlayerThrow _playerThrow;
    private bool isActive = false;

    [FormerlySerializedAs("_playerMovement")] [SerializeField] private SamplePlayerMovement samplePlayerMovement;

    void Start()
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
        _playerDrop.ActivateAction();
        _playerThrow.ActivateAction();
        _playerShoot.ActivateAction();
        _playerTake.ActivateAction();
        _playerMovement.ActivateAction();
    }

	public void DeactivateController(){
		isActive = false;
		_playerDrop.DeactivateAction();
		_playerThrow.DeactivateAction();
		_playerShoot.DeactivateAction();
		_playerTake.DeactivateAction();
		_playerMovement.DeactivateAction();
	}

}
