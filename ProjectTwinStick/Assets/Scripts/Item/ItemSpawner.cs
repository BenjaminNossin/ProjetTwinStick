using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemSpawner : MonoBehaviour
{
   private ItemsSpawnerManager _itemsSpawnerManager;
   public void Init(ItemsSpawnerManager manager)
   {
      _itemsSpawnerManager = manager;
   }
}
