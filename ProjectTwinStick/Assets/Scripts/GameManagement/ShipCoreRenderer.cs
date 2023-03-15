using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ShipCoreRenderer : MonoBehaviour
{
    [FormerlySerializedAs("_meshRendererOlives")] [SerializeField] private List<Olive> _olives;
    [SerializeField] private Animator _animator;
    [SerializeField] private ShipCore _shipCore;
    [SerializeField] private int currentCount; 
    void Start()
    {
        _shipCore.OnDecreaseCurrentHp += LaunchOliveDeathAnimation;
    }

    public void Init()
    {
        for (int i = 0; i < _olives.Count; i++)
        {
           _olives[i].Init();
        }

        currentCount = _olives.Count;
        _animator.Play("Idle");
    }

    private void LaunchOliveDeathAnimation(float currentHp)
    {
        Debug.Log(currentHp);
        currentCount--;
        _olives[currentCount].InitializeHitOlive();
        _animator.Play("Hit");
    }

}
