using System;
using Game.Systems.GlobalFramework;
using HelperPSR.Pool;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeteorProjectileBehaviour : MonoBehaviour
{
    [SerializeField, Range(1, 20)] float unitsPerSeconds = 10;
    [SerializeField, Range(1, 20)] float damage = 1f;
    [SerializeField] private GameplayTag immuneToMeteorTag;
    [SerializeField] private SlowSO playerSlowEffect_Stun;

    [SerializeField] private Collider _collider;
    [SerializeField] private float deathTime;
    private bool isDied;
    public Pool<MeteorProjectileBehaviour> _pool;
    private Transform cachedTransf;

    private Vector3 targetPos;
    private Vector3 normalizedDirection;

    private List<Transform> possibleTargets = new();

    private Vector3 selfPosFlat, shipCorePos;

    public event Action deathByShield;
    public event Action deathByBarricadeOrBaseOrPlayer;
    private void Start()
    {
        GameManager.Instance.OnGameOverCallBack += DieImmediatly;
        shipCorePos = GameManager.Instance.ShipCoreObj.transform.position;
    }

    public void Init(Vector3 assignedTargetPos)
    {
        cachedTransf = transform;
        SetTargetPosition(assignedTargetPos);
        SetSelfPosFlat(shipCorePos.y*1.1f);
        SetNormalizedDirection();
        transform.forward = normalizedDirection;
        _collider.enabled = true;
        isDied = false;
    }

    void Update()
    {
        if(isDied) return;
        Move();
    }

    GameplayTagContainer gtc; 
    private void OnTriggerEnter(Collider other)
    {
        // shield
        if (other.CompareTag("ShieldInstance"))
            {
                Debug.Log("entity with ignore meteorite tag was detected");
                deathByShield?.Invoke();
                DieImmediatly();
                return; 
            }

        // barricade or ship core
        if (other.TryGetComponent<ILifeable>(out var lifeable))
        {
            ApplyEffectOnTarget(lifeable);
        }
        else
        {
            lifeable = other.GetComponentInParent<ILifeable>();

            if (lifeable != null)
            {
                ApplyEffectOnTarget(lifeable); 
            }
        }

        // player
        if (other.TryGetComponent<SlowManager>(out var slowManager))
        {
            Debug.Log("slowing target with " + playerSlowEffect_Stun.name);
            slowManager.AddSlow(playerSlowEffect_Stun);
            deathByBarricadeOrBaseOrPlayer?.Invoke();
            DieImmediatly();
        }
    }

    private void ApplyEffectOnTarget(ILifeable lifeable)
    {
        Debug.Log("damaging target");
        deathByBarricadeOrBaseOrPlayer?.Invoke();
        DamageTarget(lifeable);
        DieImmediatly();
    }

    private void DamageTarget(ILifeable lifeable)
    {
        lifeable.DecreaseCurrentHp(damage);
    }

    private void Move()
    {
        Debug.DrawRay(transform.position, normalizedDirection * 3f, Color.blue, Time.deltaTime);
        transform.Translate(Time.deltaTime * unitsPerSeconds * normalizedDirection, Space.World);
    }
    
    private void SetTargetPosition(Vector3 posToReach)
    {
        targetPos = posToReach;
    }

    private void SetSelfPosFlat(float flattenValue)
    {
        selfPosFlat = new Vector3(cachedTransf.position.x, flattenValue, cachedTransf.position.z);
        cachedTransf.position = selfPosFlat;
    }

    private void SetNormalizedDirection()
    {
        normalizedDirection = (targetPos - cachedTransf.position).normalized;
    }
    private void DieImmediatly()
    {
        _pool.AddToPool(this);
        _collider.enabled = false;
        isDied = true;
    }
    private void Die()
    {
        StartCoroutine(_pool.AddToPoolLatter(this, deathTime));
        isDied = true;
        _collider.enabled = false;
    }
}
