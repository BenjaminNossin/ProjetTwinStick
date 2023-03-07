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
    public void MakeAction()
    {
    
    }

    public void MakeAction(params object[] arguments)
    {
   
    }

    public void SetupAction(params object[] arguments)
    {
      
    }

    public void DeactivateAction()
    {
    
    }

    public void ActivateAction()
    {
        if (_inventory.CurrentItem != null) return;
        
        if (Physics.SphereCast(transform.position, castRadius, transform.forward, out RaycastHit hit, 0f))
        {
            if (hit.collider.TryGetComponent(out ITakeable takeable))
            {
                Debug.Log("Hit takeable");
                if (takeable is Item inventoryItem)
                {
                    _inventory.SetItem(inventoryItem);
                    takeable.Take(gameObject);
                }
                else
                {
                    takeable.Take(gameObject);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, castRadius);
    }

    public event Action MakeActionEvent;
}
