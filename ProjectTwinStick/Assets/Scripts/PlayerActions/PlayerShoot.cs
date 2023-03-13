using System;
using System.Collections;
using System.Collections.Generic;
using HelperPSR.Pool;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShoot : MonoBehaviour, IPlayerAction
{
    public bool IsInAction { get => isInShoot; }
    private bool isInShoot;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private GameplayTagContainer _tagContainer;
    [SerializeField] private GameplayTag ShootBlocker;
    [SerializeField] PlayerStats _playerStats;
    private Vector2 inputs;

    public void PerformAction(params object[] arguments)
    {
        inputs = (Vector2)arguments[0];
    }

    public void CancelAction(params object[] arguments)
    {
        
    }

    private void Update()
    {
        if(_tagContainer.HasTag(ShootBlocker))
        {
            CancelShoot();
            return;
        }
        if (inputs.magnitude > _playerStats.AimInputThreshold)
        {
            _inventory.CurrentItem.Shoot(transform.position, inputs);
            if (!isInShoot)
            {
                PerformEvent?.Invoke();
            }
            isInShoot = true;
        }
        else
        {
            CancelShoot();
        }
    }

    private void CancelShoot()
    {
        if (isInShoot)
        {
            CancelEvent?.Invoke();
        }

        isInShoot = false;
    }

    public void SetupAction(params object[] arguments)
    {
    }

    public void DisableAction()
    {
    }

    public void EnableAction()
    {
    }

    public UnityEvent PerformEvent { get; }
    public UnityEvent CancelEvent { get; }

    [SerializeField]
    private UnityEvent _performEvent;
    [SerializeField]
    private UnityEvent _pancelEvent;

}