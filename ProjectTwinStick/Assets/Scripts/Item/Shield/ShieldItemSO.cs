using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShieldItemUpgrade : ItemUpgrade
{
    public float shieldCooldown;
}

[CreateAssetMenu(fileName = "ShieldItem", menuName = "Items/ShieldItem", order = 1)]
public class ShieldItemSO : ItemSO
{ 
    [SerializeField] private ShieldItemUpgrade[] _upgrades;
    public float ShieldHeight = 1f;
    public override ItemUpgrade[] GetUpgrades()
    {
        return _upgrades;
    }
}
