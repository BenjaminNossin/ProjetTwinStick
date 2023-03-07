using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour, IShootable, IDropable, ITakeable, IThrowable
{
    private ItemSO _so;

    public abstract ItemSO GetSO();
    public abstract void Shoot();

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

    public void Take(GameObject holder)
    {
        transform.parent = holder.transform;
    }
}