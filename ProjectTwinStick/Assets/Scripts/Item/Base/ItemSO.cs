using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    public float UpgradeResetTimer = 5f;
    public abstract ItemUpgrade[] GetUpgrades();
    public Item itemPrefab;
    public float RespawnTime;

}
