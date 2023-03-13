using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldInstance : MonoBehaviour, ILifeable
{
    [SerializeField] GameObject shieldCollider;
    private ShieldItemUpgrade _upgrade;
    private float DisabledCooldown = 0f;

    private bool isDisabled = false;
    private bool isInUse = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void startUsing()
    {
        isInUse = true;
        RefreshShieldState();
    }

    public void stopUsing()
    {
        isInUse = false;
        RefreshShieldState();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (isDisabled)
        {
            DisabledCooldown -= Time.deltaTime;
            if(DisabledCooldown <= 0)
            {
                isDisabled = false;
                shieldCollider.SetActive(true);
                RefreshShieldState();
            }
        }
    }

    private void RefreshShieldState()
    {
        if (!isDisabled && isInUse)
        {
            shieldCollider.SetActive(true);
        }
        else
        {
            shieldCollider.SetActive(false);
        }
    }

    private void DisableShield()
    {
        isDisabled = true;
        shieldCollider.SetActive(false);
        DisabledCooldown = _upgrade.shieldCooldown;
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
