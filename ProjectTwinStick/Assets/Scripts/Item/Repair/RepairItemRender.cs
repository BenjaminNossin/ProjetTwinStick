using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RepairItemRender : MonoBehaviour
{
    [SerializeField] private RepairItem _item;
    [SerializeField] private ParticleSystem repairFx;
    [SerializeField] private ParticleSystem shootFx;

    private void Start()
    {
    _item.OnFailToRepair += DeactivateRepairFx;
        _item.OnRepair += ActivateRepairFx;
        _item.OnUnShoot += DeactivateRepairItemFx;
    }
    
    private void DeactivateRepairFx()
    {
      
        repairFx.gameObject.SetActive(false);
        shootFx.gameObject.SetActive(true);
    }
    
    private void DeactivateRepairItemFx()
    {
      
        repairFx.gameObject.SetActive(false);
        shootFx.gameObject.SetActive(false);
    }

    private void ActivateRepairFx()
    {    
        repairFx.gameObject.SetActive(true);
        shootFx.gameObject.SetActive(false);
    }
    
}