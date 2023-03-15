
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
    public event Action OnRepair;
    public event Action OnFailToRepair;
    public override ItemSO GetSO()
    {
        return ItemSO;
    }

    public override bool TryShoot(Vector3 startPosition, Vector2 direction)
    {
        lastStartPos = startPosition;
        lastDirection = direction;
        RaycastHit hit;
        bool isRepair = false;
        if (Physics.Raycast(startPosition, new Vector3(direction.x,0,direction.y), out hit, currentUpgrade.Range, RepairableLayer, QueryTriggerInteraction.Collide))
        {
            Debug.Log("Hit");
            Barricade barricade = hit.rigidbody.GetComponent<Barricade>();
            if (barricade != null)
            {
                OnRepair?.Invoke();
                isRepair = true; 
                barricade.IncreaseCurrentHp(currentUpgrade.HealRate * Time.deltaTime);
            }
        }
        if(!isRepair)
        OnFailToRepair?.Invoke();
        OnShoot?.Invoke();
        return true;
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
