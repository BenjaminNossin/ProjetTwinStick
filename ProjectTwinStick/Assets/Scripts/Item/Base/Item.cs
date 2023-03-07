using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour, IShootable, IDropable, ITakeable, IThrowable, IUpgradable
{
    private int upgradeCount;
    private const int upgradeMaxCount = 3;

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
        transform.parent = null;
    }

    public virtual void Take()
    {
    }

    public virtual void Throw()
    {
    }

    public virtual bool CanTake()
    {
        return true;
    }

    public void Take(GameObject holder)
    {
        transform.parent = holder.transform;
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