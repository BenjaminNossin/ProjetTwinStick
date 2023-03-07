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
    [SerializeField, Range(0.0001f, 1f)] float InputThreshold = 0.1f;
    private Vector2 inputs;

    public void MakeAction(params object[] arguments)
    {
        inputs = (Vector2)arguments[0];
    }

    private void Update()
    {
        if (inputs.magnitude > InputThreshold)
        {
            _inventory.CurrentItem.Shoot(transform.position, inputs);
            transform.forward = new Vector3(inputs.x, 0, inputs.y);
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

    public void DeactivateAction()
    {
    }

    public void ActivateAction()
    {
    }

    public event Action MakeActionEvent;
}