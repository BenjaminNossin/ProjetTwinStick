using System;
using System.Collections;
using System.Collections.Generic;
using HelperPSR.Pool;
using UnityEngine;

public class ShooterItem : Item
{
    public ShooterItemSO so;
    private Pool<Bullet> _bulletPool;
    public override ItemSO GetSO()
    {
        return so;
    }

    public override void Shoot()
    {
       Bullet bullet = _bulletPool.GetFromPool();
       
        
    }

    private void Start()
    {
        _bulletPool = new Pool<Bullet>(so.bulletPrefab, so.StartCount);
    }
    
    
}
