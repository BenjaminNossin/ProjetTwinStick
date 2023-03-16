using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShieldInstance : MonoBehaviour, ILifeable
{

    private ShieldItemUpgrade _upgrade;
    
    
    public float DisabledCooldown { get; private set; } = 0f;


    //enabled : currently blocking enemies and meteors, broken : hit by enemy, on cooldown, disabled : not blocking anything
    public enum ShieldInstanceState
    {
        Enabled,
        Broken,
        Disabled
    }
    
    public ShieldInstanceState _shieldInstanceState = ShieldInstanceState.Enabled;

    public UnityEvent OnShieldEnabled;
    public UnityEvent OnShieldDisabled;
    public UnityEvent OnShieldBroken;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void startUsing()
    {
        if (DisabledCooldown <= 0.00001f)
        {
            SwitchState(ShieldInstanceState.Enabled);
        }
        else SwitchState(ShieldInstanceState.Broken);
    }

    public void stopUsing()
    {
        SwitchState(ShieldInstanceState.Disabled);
    }

    private void SwitchState(ShieldInstanceState state)
    {
        if (state == _shieldInstanceState) return;
        _shieldInstanceState = state;
        switch (_shieldInstanceState)
        {
            case ShieldInstanceState.Enabled:
      
                OnShieldEnabled?.Invoke();
                Debug.Log("Shield enabled");
                break;
            case ShieldInstanceState.Broken:
              
                OnShieldBroken?.Invoke();
                Debug.Log("Shield broken");
                break;
            case ShieldInstanceState.Disabled:
             
                OnShieldDisabled?.Invoke();
                Debug.Log("Shield disabled");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(_shieldInstanceState)
        {
            case ShieldInstanceState.Enabled:
                break;
            case ShieldInstanceState.Broken:
                BrokenUpdate();
                break;
            case ShieldInstanceState.Disabled:
                break;
        }
    }

    private void BrokenUpdate()
    {
        DisabledCooldown -= Time.deltaTime;
        if(DisabledCooldown <= 0)
        {
            SwitchState(ShieldInstanceState.Enabled);
        }
    }
    

    private void DisableShield()
    {
        DisabledCooldown = _upgrade.shieldCooldown;
        SwitchState(ShieldInstanceState.Broken);
    }

    public void ChangeUpgrade(ShieldItemUpgrade upgrade)
    {
        _upgrade = upgrade;
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
        DisableShield();
    }

    public event Action<float> OnDecreaseCurrentHp;
}
