using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerThrow : MonoBehaviour, IPlayerAction
{
    
    [SerializeField] float ThrowChargeTime = 1f;


    public UnityEvent OnAimStart;
    public UnityEvent OnThrow;
    public UnityEvent OnCancelThrow;
    
    public bool IsInAction { get; }
    
    private float currentChargeTime = 0f;
    private bool IsPreparingThrow;
    
    [SerializeField] private Inventory _inventory;

    //bool start throw false, finish throw true
    public void PerformAction(params object[] arguments)
    {
        bool ThrowInput = (bool) arguments[0];
        if (ThrowInput == false)
        {
            TryAim();
        }
        else
        {
            TryPerformThrow();
        }
    }

    private void TryAim()
    {
        if (!_inventory.IsDefaultItem())
        {
            Debug.Log("trying to aim");
            IsPreparingThrow = true;
            OnAimStart?.Invoke();
        }
    }

    private void TryPerformThrow()
    {
        if (!_inventory.IsDefaultItem())
        {
            Debug.Log("trying to throw");
            IsPreparingThrow = false;
            _inventory.CurrentItem.Throw(currentChargeTime);
            _inventory.ClearItem(false);
        }
    }

    private void CancelThrow()
    {
        if (IsPreparingThrow)
        {
            Debug.Log("cancel throw");
            IsPreparingThrow = false;
            currentChargeTime = 0f;
        }
    }

    private void Update()
    {
        if (IsPreparingThrow)
        {
            currentChargeTime = Mathf.MoveTowards(currentChargeTime, 1, Time.deltaTime / ThrowChargeTime);
        }
    }

    public void CancelAction(params object[] arguments)
    {
        CancelThrow();
    }

    public void SetupAction(params object[] arguments)
    {
        _inventory.OnItemChanged += OnItemChange;
    }

    private void OnItemChange(Item obj)
    {
        CancelThrow();
    }

    public void DisableAction()
    {
 
    }

    public void EnableAction()
    {
        
    }

    public event Action PerformActionEvent;
}
