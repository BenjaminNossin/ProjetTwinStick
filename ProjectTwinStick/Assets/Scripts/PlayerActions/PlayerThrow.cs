using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerThrow : MonoBehaviour, IPlayerAction
{
    [SerializeField] ItemThrowData throwData;
    [SerializeField] GameplayTag MovementBlocker;
    [SerializeField] GameplayTag PickupBlocker;
    [SerializeField] GameplayTag ShootBlocker;
    [SerializeField]  GameplayTag AimSlower;


    public UnityEvent OnAimStart;
    public UnityEvent OnThrow;
    public UnityEvent OnCancelThrow;
    
    public bool IsInAction { get; }
    
    private float currentChargeTime = 0f;
    private bool IsPreparingThrow;
    
    [SerializeField] private Inventory _inventory;
    [SerializeField] private GameplayTagContainer _tagContainer;

    private ItemTrajectoryPreview currentPreview;

    
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
        if (!_inventory.IsDefaultItem() && !IsPreparingThrow)
        {
            Debug.Log("trying to aim");
            IsPreparingThrow = true;
            _tagContainer.AddTag(MovementBlocker);
            _tagContainer.AddTag(PickupBlocker);
            _tagContainer.AddTag(ShootBlocker);
            _tagContainer.AddTag(AimSlower);
            OnAimStart?.Invoke();
        }
    }

    private void TryPerformThrow()
    {
        if (!_inventory.IsDefaultItem() && IsPreparingThrow)
        {
            Debug.Log("trying to throw");
            IsPreparingThrow = false;
            _inventory.CurrentItem.Throw(currentChargeTime, transform.forward);
            _inventory.ClearItem(false);
            currentChargeTime = 0f;
            _tagContainer.RemoveTag(MovementBlocker);
            _tagContainer.RemoveTag(PickupBlocker);
            _tagContainer.RemoveTag(ShootBlocker);
            _tagContainer.RemoveTag(AimSlower);
        }
    }

    private void CancelThrow()
    {
        if (IsPreparingThrow)
        {
            Debug.Log("cancel throw");
            IsPreparingThrow = false;
            currentChargeTime = 0f;
            _tagContainer.RemoveTag(MovementBlocker);
            _tagContainer.RemoveTag(PickupBlocker);
            _tagContainer.RemoveTag(ShootBlocker);
            _tagContainer.RemoveTag(AimSlower);
        }
    }

    private void Update()
    {
        if (IsPreparingThrow)
        {
            currentChargeTime = Mathf.MoveTowards(currentChargeTime, 1, Time.deltaTime / throwData.ThrowChargeTime);
            Debug.Log(currentChargeTime);
            currentPreview?.UpdatePreview(transform.position, transform.forward, currentChargeTime);
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
        currentPreview = obj.GetComponent<ItemTrajectoryPreview>();
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
