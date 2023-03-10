
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairItem : Item
{
    [SerializeField] RepairItemSO ItemSO;
    [SerializeField] private LayerMask RepairableLayer;
    private RepairItemUpgrade currentUpgrade;

    private Vector3 lastStartPos;
    private Vector3 lastDirection;
    
    public override ItemSO GetSO()
    {
        return ItemSO;
    }

    public override void Shoot(Vector3 startPosition, Vector2 direction)
    {
        lastStartPos = startPosition;
        lastDirection = direction;
        RaycastHit hit;
        if (Physics.Raycast(startPosition, new Vector3(direction.x,0,direction.y), out hit, currentUpgrade.Range, RepairableLayer, QueryTriggerInteraction.Collide))
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
            Gizmos.DrawLine(lastStartPos, lastStartPos + new Vector3(lastDirection.x,0,lastDirection.y) * currentUpgrade.Range);
        }
    }
}
