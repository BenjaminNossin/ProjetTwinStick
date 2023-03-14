using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Item DefaultItem;
    public Item CurrentItem { get; private set; }
    
    public event Action<Item> OnItemChanged;

  public  event Action OnSetDefaultItem;
    public event Action<Item> OnItemDroped; 
    private void Start()
    {
       ClearItem(false);
    }

    public void SetItem(Item item)
    {
        if (CurrentItem != null && CurrentItem != DefaultItem)
        {
            OnItemDroped?.Invoke(CurrentItem);
            CurrentItem.Drop();
        }
        CurrentItem = item;
        OnItemChanged?.Invoke(CurrentItem);
    }
    
    public void ClearItem(bool DropItem)
    {
        if (CurrentItem != null && CurrentItem != DefaultItem && DropItem)
        {
            CurrentItem.Drop();
        }
        CurrentItem = DefaultItem;
        OnSetDefaultItem?.Invoke();
        
    }

    public bool IsDefaultItem()
    {
        return DefaultItem == CurrentItem;
    }
}
