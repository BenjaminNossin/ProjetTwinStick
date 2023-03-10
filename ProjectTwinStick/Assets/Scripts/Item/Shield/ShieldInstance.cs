using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldInstance : MonoBehaviour, ILifeable
{
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetMaxHp()
    {
        throw new NotImplementedException();
    }

    public float GetCurrentHp()
    {
        throw new NotImplementedException();
    }

    public void SetMaxHp(float value)
    {
        
    }

    public void IncreaseMaxHp(float amount)
    {
    }

    public event Action<float> OnSetMaxHp;
    public event Action<float> OnIncreaseMaxHp;
    public void DecreaseMaxHp(float amount)
    {
        throw new NotImplementedException();
    }

    public event Action<float> OnDecreaseMaxHp;
    public void SetCurrentHp(float value)
    {
        throw new NotImplementedException();
    }

    public event Action<float> OnSetCurrentHp;
    public void IncreaseCurrentHp(float amount)
    {
        throw new NotImplementedException();
    }

    public event Action<float> OnIncreaseCurrentHp;
    public void DecreaseCurrentHp(float amount)
    {
        throw new NotImplementedException();
    }

    public event Action<float> OnDecreaseCurrentHp;
}
