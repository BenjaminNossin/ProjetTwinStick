using Game.Systems.GlobalFramework;
using HelperPSR.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorProjectileBehaviour : MonoBehaviour
{
    [SerializeField, Range(1, 20)] float unitsPerSeconds = 10;
    [SerializeField, Range(1, 20)] float damage = 20f;

    public Pool<MeteorProjectileBehaviour> _pool;
    private Transform cachedTransf;

    private Vector3 targetPos;
    private Vector3 normalizedDirection;


    private void Start()
    {
        GameManager.Instance.OnGameOverCallBack += Die;
    }

    public void Init(Vector3 assignedBarricadePos)
    {
        cachedTransf = transform;

        SetTargetPosition(assignedBarricadePos);
        SetNormalizedDirection();
    }

    void Update()
    {
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        // if player -> not lifeable. Use SlowManager
        // for now, touching an enemy kills it
        if (other.TryGetComponent<ILifeable>(out var lifeable))
        {
            //Debug.Log($"damaging target {other.gameObject.name}"); 
            DamageTarget(lifeable);
            Die();
        }
        else
        {
            lifeable = other.GetComponentInParent<ILifeable>();
            if (lifeable != null)
            {
                //Debug.Log($"damaging target {other.gameObject.name}"); 
                DamageTarget(lifeable);
                Die();
            }
        }
    }

    private void DamageTarget(ILifeable lifeable)
    {
        lifeable.DecreaseCurrentHp(damage);
    }

    private void Move()
    {
        Debug.DrawRay(cachedTransf.position, normalizedDirection * 3f, Color.blue, Time.deltaTime);
        cachedTransf.Translate(Time.deltaTime * unitsPerSeconds * normalizedDirection, Space.Self);
    }

    private void SetTargetPosition(Vector3 posToReach)
    {
        targetPos = posToReach;
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
