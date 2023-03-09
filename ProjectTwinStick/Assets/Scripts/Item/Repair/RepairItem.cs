
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairItem : Item
{
    [SerializeField] RepairItemSO ItemSO;
    [SerializeField] private LayerMask RepairableLayer;
    private RepairItemUpgrade currentUpgrade;
    
    public override ItemSO GetSO()
    {
        return ItemSO;
    }

    public override void Shoot(Vector3 startPosition, Vector2 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(startPosition, direction, out hit, currentUpgrade.Range, RepairableLayer))
        {
            Barricade barricade = hit.collider.GetComponent<Barricade>();
            if (barricade != null)
            {
                barricade.IncreaseCurrentHp(currentUpgrade.HealRate * Time.deltaTime);
            }
        }
    }

    public override void SetUpgrade(ItemUpgrade newUpgrade)
    {
        currentUpgrade = (RepairItemUpgrade)newUpgrade;
    }

    private void OnDrawGizmos()
    {
        if (currentUpgrade != null && _itemHolder != null && CurrentItemState == ItemState.Held)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_itemHolder.transform.position, _itemHolder.transform.position + _itemHolder.transform.forward * currentUpgrade.Range);
        }
    }
}
