using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ShooterItem", menuName = "Items/ShooterItem", order = 1)]
public class ShooterItemSO : ItemSO
{
    [SerializeField]
    private ShooterUpgrade[] _shooterUpgrades;
    public Bullet bulletPrefab;
    public int StartCount;
    private void OnValidate()
    {
        if(_shooterUpgrades.Length ==0)return;
        foreach (var upgrade in _shooterUpgrades)
        {
        upgrade.MaxDispersionRadian = upgrade.MaxDispersionDegres * Mathf.Deg2Rad;
        upgrade.MinDispersionRadian = upgrade.MinDispersionDegres * Mathf.Deg2Rad;
        }
    }

    public override ItemUpgrade[] GetUpgrades()
    {
        return _shooterUpgrades;
    }
}