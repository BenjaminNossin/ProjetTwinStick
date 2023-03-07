using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterItem : Item
{
    public ShooterItemSO so;
    public override ItemSO GetSO()
    {
        return so;
    }

    public override void Shoot()
    {
        throw new System.NotImplementedException();
    }
}
