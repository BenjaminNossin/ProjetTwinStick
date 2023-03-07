using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerTake : MonoBehaviour, IPlayerAction
{
    [SerializeField] private float castRadius = 1f;
    [SerializeField] private LayerMask takeableLayer;

    [SerializeField] private Inventory _inventory;
    
    public bool IsInAction { get; }

    public void MakeAction(params object[] arguments)
    {
        if (_inventory.CurrentItem != null && !_inventory.IsDefaultItem()) return;

        Debug.Log("trying to find item to take");

        Collider[] colliders = Physics.OverlapSphere(transform.position, castRadius, takeableLayer);

        if (colliders.Length != 0)
        {
            Debug.Log("Hit something");
            if (colliders[0].TryGetComponent(out ITakeable takeable))
            {
                Debug.Log("Hit takeable");
                if (takeable is Item inventoryItem)
                {
                    _inventory.SetItem(inventoryItem);
                }
                takeable.Take(gameObject);
            }
        }
    }

    public void SetupAction(params object[] arguments)
    {
      
    }

    public void DeactivateAction()
    {
    
    }

    public void ActivateAction()
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, castRadius);
    }

    public event Action MakeActionEvent;
}
