using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterItemRender : MonoBehaviour
{
    [SerializeField]
    private ShooterItem _item;

    [SerializeField] private ParticleSystem shootFxPrefab;
    private void OnEnable()
    {
        _item.OnShoot += LaunchShootFx;
    }

    private void LaunchShootFx()
    {
        if(!shootFxPrefab) return;
      var fx =  PoolFeedbackManager.instance.GetFromPool(shootFxPrefab.gameObject,shootFxPrefab.main.duration);
        fx.transform.position = _item.shootPivotPoint.transform.position;
        fx.transform.forward = _item.transform.forward;

    }
}
  