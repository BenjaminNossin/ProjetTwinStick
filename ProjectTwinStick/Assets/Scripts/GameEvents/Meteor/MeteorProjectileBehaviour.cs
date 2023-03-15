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

    public Pool<MeteorProjectileBehaviour> _pool;
    private Transform cachedTransf;

    private Vector3 targetPos;
    private Vector3 normalizedDirection;

    private List<Transform> possibleTargets = new();

    private Vector3 selfPosFlat, shipCorePos; 

    private void Start()
    {
        GameManager.Instance.OnGameOverCallBack += Die;
        shipCorePos = GameManager.Instance.ShipCoreObj.transform.position;
    }

    public void Init(Vector3 assignedTargetPos)
    {
        cachedTransf = transform;
        SetTargetPosition(assignedTargetPos);
        SetSelfPosFlat(shipCorePos.y*1.1f);
        SetNormalizedDirection();
        transform.forward = normalizedDirection;
    }

    void Update()
    {
        Move();
    }

    GameplayTagContainer gtc; 
    private void OnTriggerEnter(Collider other)
    {
        // shield
        if (other.TryGetComponent<GameplayTagContainer>(out var gameplayTagContainer))
        {
            if (gameplayTagContainer.HasTag(immuneToMeteorTag))
            {
                Debug.Log("entity with ignore meteorite tag was detected");

                Die();
                return; 
            }
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
            Die();
        }
    }

    private void ApplyEffectOnTarget(ILifeable lifeable)
    {
        Debug.Log("damaging target");

        DamageTarget(lifeable);
        Die();
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

    private void Die()
    {
        _pool.AddToPool(this);
    }
}
