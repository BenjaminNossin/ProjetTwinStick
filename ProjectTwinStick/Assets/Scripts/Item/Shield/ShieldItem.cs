using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShieldItem : Item
{
    private ShieldItemSO _shieldItemSO;
    private ShieldItemUpgrade _currentUpgrade;
    [SerializeField]
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
        if(state != ItemState.Held)
        {
            DisableShield();
        }
        else EnableShield();
    }

    private void DisableShield()
    {
        _shieldInstance.stopUsing();
    }

    private void EnableShield()
    {
        _shieldInstance.startUsing();
    }

    public override void Shoot(Vector3 startPosition, Vector2 direction)
    {
        //TODO : Raycast to edge of map instead
        direction.Normalize();
        _shieldInstance.transform.position = startPosition + new Vector3(direction.x, 0, direction.y) * 5f;
        _shieldInstance.transform.forward = new Vector3(direction.x, 0, direction.y);
    }

    public override void SetUpgrade(ItemUpgrade newUpgrade)
    {
        _currentUpgrade = (ShieldItemUpgrade) newUpgrade;
        _shieldInstance.ChangeUpgrade(_currentUpgrade);
    }
}
