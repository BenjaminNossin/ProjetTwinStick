using System;
using System.Collections;
using System.Collections.Generic;
using HelperPSR.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShooterItem : Item
{
    public ShooterItemSO so;
    private Pool<Bullet> _bulletPool;
    private float fireRateTimer;
    private bool shooterIsReady = true;
    private ShooterUpgrade currentUpgrade = new ShooterUpgrade();

    [SerializeField] AudioSource audioSource;
    [SerializeField] SoundPitch pitcher;
    public override ItemSO GetSO()
    {
        return so;
    }

    protected override void Update()
    {
        base.Update();
        if (!shooterIsReady)
        {
            if (fireRateTimer < currentUpgrade.FrequencyBetweenBullet)
            {
                fireRateTimer += Time.deltaTime;
            }
            else
            {
                shooterIsReady = true;
                fireRateTimer = 0;
            }
        }
    }

    public override bool TryShoot(Vector3 startPosition, Vector2 direction)
    {
        if (!shooterIsReady)
        {
            CancelShoot();
            return false;
        }
        Bullet bullet = _bulletPool.GetFromPool();
        var currentAngle = Mathf.Atan2(direction.y, direction.x);
        currentAngle += Random.Range(currentUpgrade.MinDispersionRadian, currentUpgrade.MaxDispersionRadian);
        bullet.Init(shootPivotPoint.transform.position,currentUpgrade.DamageBullet, currentUpgrade.Speed,
            new Vector3(Mathf.Cos(currentAngle), 0, Mathf.Sin(currentAngle)),currentUpgrade.slowType, _bulletPool);
        shooterIsReady = false;
        OnShoot?.Invoke();
        pitcher.Pitcher();
        audioSource.Play();
        return true;
    }

    public override void SetUpgrade(ItemUpgrade newUpgrade)
    {
        currentUpgrade =(ShooterUpgrade) newUpgrade;
    }

    protected override void Start()
    {
        base.Start();
        _bulletPool = new Pool<Bullet>(so.bulletPrefab, so.StartCount);
    }
}