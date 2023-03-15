using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRenderer : MonoBehaviour
{
    [SerializeField] private ParticleSystem fxImpactPrefab;

    [SerializeField] private Bullet _bullet;
    public void Init()
    {
        _bullet.OnDieBullet += LaunchImpactFx;
    }

    private void LaunchImpactFx()
    {
        var fx =  PoolFeedbackManager.instance.GetFromPool(fxImpactPrefab.gameObject,fxImpactPrefab.main.duration);
        fx.transform.position = _bullet.transform.position;
        fx.transform.forward = _bullet.transform.forward;
    }
    
}
