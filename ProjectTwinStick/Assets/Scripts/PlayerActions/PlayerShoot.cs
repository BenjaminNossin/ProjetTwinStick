using System;
using System.Collections;
using System.Collections.Generic;
using HelperPSR.Pool;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShoot : MonoBehaviour, IPlayerAction
{
    public bool IsInAction
    {
        get => isInShoot;
    }

    private bool isInShoot;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private GameplayTagContainer _tagContainer;
    [SerializeField] private GameplayTag ShootBlocker;
    [SerializeField] PlayerStats _playerStats;
    private Vector2 inputs;
    public event Action<bool> OnTryShoot;
    public event Action OnCancelShoot;

    public void PerformAction(params object[] arguments)
    {
        inputs = (Vector2)arguments[0];
    }

    public void CancelAction(params object[] arguments)
    {
    }

    private void Update()
    {
        if (_tagContainer.HasTag(ShootBlocker))
        {
            CancelShoot();
            return;
        }

        if (inputs.magnitude > _playerStats.AimInputThreshold)
        {
            OnTryShoot?.Invoke(_inventory.CurrentItem.TryShoot(transform.position, inputs));

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
            _inventory.CurrentItem.CancelShoot();
        OnCancelShoot?.Invoke();
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
}