using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemState
{
    Held,
    Thrown,
    Bouncing,
    Dropped
}

public abstract class Item : MonoBehaviour, IShootable, IDropable, ITakeable, IThrowable, IUpgradable
{
    private int upgradeCount;
    private const int upgradeMaxCount = 3;

    [Header("throw")] 
    private float ThrowLength;

    private AnimationCurve ThrowCurve;
    private AnimationCurve BounceCurve;


    private GameObject itemHolder;

    protected ItemState CurrentItemState { get; private set; }

    private void ChangeState(ItemState newState)
    {
        CurrentItemState = newState;
        switch (CurrentItemState)
        {
            case ItemState.Held:
                OnHeld();
                break;
            case ItemState.Thrown:
                OnThrown();
                break;
            case ItemState.Bouncing:
                OnBouncing();
                break;
            case ItemState.Dropped:
                OnDropped();
                break;
        }
    }

    private void OnThrown()
    {
        
    }

    private void OnBouncing()
    {
        
    }

    private void OnHeld()
    {
        
        transform.parent = itemHolder.transform;
    }
    
    private void OnDropped()
    {
        Vector3 position = itemHolder.transform.parent.position;
        transform.position = new Vector3(position.x, 1, position.z);
        transform.parent = null;
    }

    public abstract ItemSO GetSO();
    public abstract void Shoot(Vector3 startPosition, Vector2 direction);

    public abstract void SetUpgrade(ItemUpgrade newUpgrade);
    protected virtual void Start()
    {
       ResetUpgrade();
    }

    public virtual bool CanDrop()
    {
        return true;
    }

    public virtual void Drop()
    {
        ChangeState(ItemState.Dropped);
    }

    public virtual void Throw()
    {
    }

    public virtual bool CanTake()
    {
        return CurrentItemState != ItemState.Held;
    }

    public void Take(GameObject holder)
    {
        itemHolder = holder;
        ChangeState(ItemState.Held);
    }

    public void Upgrade()
    {
        if (upgradeCount != upgradeMaxCount)
        {
            upgradeCount++;
            UpdateUpgrade();
        }
    }

    public void Degrade()
    {
        if (upgradeCount != 0)
        {
            upgradeCount--;
            UpdateUpgrade();
        }
    }

    void UpdateUpgrade() => SetUpgrade(GetSO().GetUpgrades()[upgradeCount]);

    public void ResetUpgrade()
    {
        upgradeCount = 0;
        UpdateUpgrade();
    }
}