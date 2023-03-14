using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRender : MonoBehaviour
{
    private Item _item;

    [SerializeField] private GameObject dropStateFx;
    private void OnEnable()
    {

        _item = GetComponent<Item>();
        _item.OnItemStateChange += OnItemStateChange;
    }
    
    private void OnDisable()
    {
  
        _item.OnItemStateChange -= OnItemStateChange;
    }

    private void OnItemStateChange(ItemState state)
    {
        switch (state)
        {
            case ItemState.Bouncing :
            {
                dropStateFx.SetActive(false);
                break;
            }
            case ItemState.Dropped:
            {
                dropStateFx.SetActive(true);
                break;
            }
            case ItemState.Thrown:
            {
                dropStateFx.SetActive(false);
                break;
            }
            case ItemState.Held:
            {
                dropStateFx.SetActive(false);
                break;
            }

        }
    }

}
