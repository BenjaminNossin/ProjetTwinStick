using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShieldItemUpgrade : ItemUpgrade
{
    public float shieldWidth;
    public float shieldCooldown;
}

public class ShieldItemSO : ItemSO
{ 
    [SerializeField] private ShieldItemUpgrade[] _upgrades;
    public override ItemUpgrade[] GetUpgrades()
    {
        return _upgrades;
    }
}
