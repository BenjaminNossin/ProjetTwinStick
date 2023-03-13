using Game.Systems.GlobalFramework;
using HelperPSR.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorProjectileBehaviour : MonoBehaviour
{
    [SerializeField, Range(1, 20)] float unitsPerSeconds = 10;

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
