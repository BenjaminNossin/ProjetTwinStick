using Game.Systems.GlobalFramework;
using HelperPSR.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorProjectileBehaviour : MonoBehaviour
{
    [SerializeField, Range(1, 20)] float unitsPerSeconds = 10;
    [SerializeField, Range(1, 20)] float damage = 1f;

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
    }

    void Update()
    {
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        // if player -> not lifeable. Use SlowManager
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
        Debug.DrawRay(transform.position, normalizedDirection * 3f, Color.blue, Time.deltaTime);
        transform.Translate(Time.deltaTime * unitsPerSeconds * normalizedDirection, Space.Self);
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
