using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Olive : MonoBehaviour
{
    public MeshRenderer _meshRenderer;
    public ParticleSystem _particleSystem;
    private bool _isInDissolve;
    [SerializeField]
    private float dissolveTime;
    private float dissolveTimer;

    public void Init()
    {
        _meshRenderer.material.SetFloat("_Hit", 0);
        _meshRenderer.material.SetFloat("_Dissolve",1 );
        _particleSystem.gameObject.SetActive(false);
        dissolveTimer = 0;
    }

    public void InitializeHitOlive()
    {
        _meshRenderer.material.SetFloat("_Hit", 1);
        _isInDissolve = true;
        _particleSystem.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (_isInDissolve)
        {
            dissolveTimer += Time.deltaTime;
            _meshRenderer.material.SetFloat("_Dissolve", 1-(dissolveTimer/dissolveTime));
            if (dissolveTimer > dissolveTime)
            {
                _isInDissolve = false;
            }
        }
    }
}
