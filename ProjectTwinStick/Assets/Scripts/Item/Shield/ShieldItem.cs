using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldItem : Item
{
    private ShieldItemSO _shieldItemSO;
    private ShieldItemUpgrade _currentUpgrade;
    private ShieldInstance _shieldInstance;
    
    public override ItemSO GetSO()
    {
        return _shieldItemSO;
    }

    protected override void Start()
    {
        OnItemStateChange += OnItemStateChanged;
    }

    private void OnItemStateChanged(ItemState state)
    {
        if(state != ItemState.Thrown)
        {
            Debug.Log("Should disable shield");
        }
    }

    public override void Shoot(Vector3 startPosition, Vector2 direction)
    {
        
    }

    public override void SetUpgrade(ItemUpgrade newUpgrade)
    {
        _currentUpgrade = (ShieldItemUpgrade) newUpgrade;
    }
}
