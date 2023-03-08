using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
    [SerializeField] private float ThrowLength = 3f;

    [SerializeField] private float ThrowHeight = 2f;
    [SerializeField] private AnimationCurve ThrowCurve;
    [SerializeField] private AnimationCurve BounceCurve;


    private GameObject itemHolder;
    private float LastThrowForce;

    private float ThrowTimer = 0;
    private float BounceTimer = 0;

    protected ItemState CurrentItemState { get; private set; } = ItemState.Dropped;

    private void ChangeState(ItemState newState)
    {
        CurrentItemState = newState;
        switch (CurrentItemState)
        {
            case ItemState.Held:
                OnHeld();
                break;
            case ItemState.Thrown:
                OnThrowStart();
                break;
            case ItemState.Bouncing:
                OnBounceStart();
                break;
            case ItemState.Dropped:
                OnDropped();
                break;
        }
    }

    private void OnThrowStart()
    {
        ThrowTimer = 0;
    }

    private void OnBounceStart()
    {
        BounceTimer = 0;
    }

    private void OnHeld()
    {
        
        transform.parent = itemHolder.transform;
    }
    
    private void OnDropped()
    {
        Vector3 position = itemHolder.transform.position;
        transform.parent = null;
        itemHolder = null;
        Debug.Log(position);
        transform.position = new Vector3(position.x, 1, position.z);
        transform.rotation = quaternion.identity;
    }

    public abstract ItemSO GetSO();
    public abstract void Shoot(Vector3 startPosition, Vector2 direction);

    public abstract void SetUpgrade(ItemUpgrade newUpgrade);
    protected virtual void Start()
    {
       ResetUpgrade();
    }

    private void Update()
    {
        switch (CurrentItemState)
        {
            case ItemState.Thrown:
                ThrowUpdate();
                break;
            case ItemState.Bouncing:
                BounceUpdate ();
                break;
        }
    }

    private void ThrowUpdate()
    {
        
    }

    private void BounceUpdate()
    {
        
    }

    public virtual bool CanDrop()
    {
        return true;
    }

    public virtual void Drop()
    {
        ChangeState(ItemState.Dropped);
    }

    public virtual void Throw(float throwForce)
    {
        LastThrowForce = throwForce;
        ChangeState(ItemState.Thrown);
        //Drop();
    }

    public virtual bool CanTake()
    {
        if (CurrentItemState != ItemState.Held)
        {
            return true;
        }

        return false;
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