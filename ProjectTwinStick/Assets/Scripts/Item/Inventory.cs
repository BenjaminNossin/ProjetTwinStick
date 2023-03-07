using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Item CurrentItem { get; private set; }
    
    public Action<Item> OnItemChanged;
    
    public void SetItem(Item item)
    {
        CurrentItem = item;
        OnItemChanged?.Invoke(CurrentItem);
    }
    
    public void ClearItem()
    {
        CurrentItem = null;
        OnItemChanged?.Invoke(CurrentItem);
    }
}
