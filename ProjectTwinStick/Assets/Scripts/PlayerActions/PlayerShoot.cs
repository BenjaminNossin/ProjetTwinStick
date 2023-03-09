using System;
using System.Collections;
using System.Collections.Generic;
using HelperPSR.Pool;
using UnityEngine;

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
            isInShoot = false;
            return;
        }
        if (inputs.magnitude > _playerStats.AimInputThreshold)
        {
            _inventory.CurrentItem.Shoot(transform.position, inputs);
            isInShoot = true;
        }
        else
        {
            isInShoot = false;
        }
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

    public event Action PerformActionEvent;
}