using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RepairItemRender : MonoBehaviour
{
    [SerializeField]
    private RepairItem _item;

   [SerializeField] private ParticleSystem repairFx;
    private void OnEnable()
    {
        _item.OnShoot += LaunchShootFx;
        _item.OnUnShoot += CancelShootFx;
    }

    private void LaunchShootFx()
    {
        if(!repairFx) return;
        repairFx.gameObject.SetActive(true);
    }

    private void CancelShootFx()
    {
        if(!repairFx) return;
        repairFx.gameObject.SetActive(false);
    }
}
