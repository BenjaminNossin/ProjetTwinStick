using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerDrop : MonoBehaviour, IPlayerAction
{
    [SerializeField] private GameplayTag DropBlocker;
    [SerializeField] private GameplayTagContainer TagContainer;
    [SerializeField] private Inventory inventory;
    
    public bool IsInAction { get; }
    
    public void MakeAction()
    {

    }

    public void PerformAction(params object[] arguments)
    {
        if (TagContainer.HasTag(DropBlocker))
        {
            return;
        }
        if (inventory.IsDefaultItem())
        {
            return;
        }
        inventory.ClearItem(true);
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


}
