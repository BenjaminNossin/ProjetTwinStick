using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorRender : MonoBehaviour
{
    [SerializeField] private MeteorProjectileBehaviour _meteorProjectileBehaviour;

    [SerializeField] private ParticleSystem _explosionFx;
    [SerializeField] private ParticleSystem _explosionFailedFx;
    
    public void Start()
    {
        _meteorProjectileBehaviour.deathByShield += InitiateDeathByShieldFx;
        _meteorProjectileBehaviour.deathByBarricadeOrBaseOrPlayer += InitiateDeathByOther;
    }

    private void InitiateDeathByShieldFx()
    {
        var fx = PoolFeedbackManager.instance.GetFromPool(_explosionFailedFx.gameObject, _explosionFailedFx.main.duration);
        fx.transform.position = transform.position;

    }

    private void InitiateDeathByOther()
    {
       var fx = PoolFeedbackManager.instance.GetFromPool(_explosionFx.gameObject, _explosionFx.main.duration);
       fx.transform.position = transform.position;
    }
    
    
    
    
    
    
}
