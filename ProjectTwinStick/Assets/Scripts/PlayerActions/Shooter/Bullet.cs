using System;
using System.Collections;
using System.Collections.Generic;
using HelperPSR.Pool;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Bullet : MonoBehaviour
{
    
    private float _damage;
    private bool _isActivate;

    private Pool<Bullet> _pool;
    [SerializeField]
    private Rigidbody _rb;
    private SlowSO _slow;
    public void Init(Vector3 startPos, float damage, float speed, Vector3 direction,SlowSO slow, Pool<Bullet> pool)
    {
        transform.position = startPos;
        _damage = damage;
        _rb.velocity = direction * speed;
        _pool = pool;
        _slow = slow;
    }
    private void OnTriggerEnter(Collider other)
    {
        var lifeable = other.GetComponent<ILifeable>();
        if (lifeable != null)
        {
            lifeable.DecreaseCurrentHp(_damage);
        }
        else
        {
            lifeable = other.GetComponentInParent<ILifeable>();
            if (lifeable != null)
            {
                lifeable.DecreaseCurrentHp(_damage);
            }
        }

        if (_slow != null)
        {
            SlowManager slowManager = other.GetComponent<SlowManager>();
            if (slowManager != null)
            {
                slowManager.AddSlow(_slow);
            }
            else
            {
                slowManager = other.GetComponentInParent<SlowManager>();
                if (slowManager != null)
                {
                    slowManager.AddSlow(_slow);
                }
            }
        }
        
        if (other.CompareTag("Wall") || other.CompareTag("Enemy"))
        {
            _pool.AddToPool(this);
        }
        
    }
}
