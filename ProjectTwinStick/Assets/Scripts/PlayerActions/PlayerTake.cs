using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerTake : MonoBehaviour, IPlayerAction
{
    [SerializeField] private float castRadius = 1f;
    [SerializeField] private LayerMask takeableLayer;

    [SerializeField] private GameplayTag PickupBlocker;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private GameplayTagContainer _tagContainer;
    
    
    public bool IsInAction { get; }

    public void PerformAction(params object[] arguments)
    {
        if (_tagContainer.HasTag(PickupBlocker))
        {
            return;
        }
        

        Collider[] colliders = Physics.OverlapSphere(transform.position, castRadius, takeableLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent(out ITakeable takeable))
            {
                if (takeable is Item inventoryItem)
                {
                    Debug.Log(inventoryItem.CanTake());
                    Debug.Log(_inventory.CurrentItem);
                    if (inventoryItem != _inventory.CurrentItem && inventoryItem.CanTake())
                    {
                        _inventory.SetItem(inventoryItem);
                        takeable.Take(gameObject);
                        break;
                    }
                }
                else
                {
                    if( takeable.CanTake())
                    {
                        takeable.Take(gameObject);
                    }
                }
            }
        }
    }

    public void CancelAction(params object[] arguments)
    {
        
    }

    public void SetupAction(params object[] arguments)
    {
      
    }

    public void DisableAction()
    {
    
    }

    public void EnableAction()
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, castRadius);
    }

    public event Action PerformActionEvent;
}
