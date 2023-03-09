using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RepairItemUpgrade : ItemUpgrade
{
    public float Range = 2f;
    public float HealRate = 1f;
}

[CreateAssetMenu(fileName = "RepairItem", menuName = "Items/RepairItem", order = 2)]
public class RepairItemSO : ItemSO
{
    [SerializeField] private RepairItemUpgrade[] _upgrades;
    public override ItemUpgrade[] GetUpgrades()
    {
        return _upgrades;
    }
}
