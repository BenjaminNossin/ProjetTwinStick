using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Item DefaultItem;
    public Item CurrentItem { get; private set; }
    
    public Action<Item> OnItemChanged;

    private void Start()
    {
        SetItem(DefaultItem);
    }

    public void SetItem(Item item)
    {
        CurrentItem = item;
        OnItemChanged?.Invoke(CurrentItem);
    }
    
    public void ClearItem()
    {
        CurrentItem = DefaultItem;
        OnItemChanged?.Invoke(CurrentItem);
    }

    public bool IsDefaultItem()
    {
        return DefaultItem == CurrentItem;
    }
}
