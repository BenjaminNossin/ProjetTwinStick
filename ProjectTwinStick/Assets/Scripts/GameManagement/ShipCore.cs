using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCore : MonoBehaviour, ILifeable
{
    [SerializeField, Range(0, 200)] private float maxHP = 100f;
    public float MaxHP { get; private set; }
    public float CurrentHP { get; private set; }

    public event Action<float> OnSetMaxHp;
    public event Action<float> OnIncreaseMaxHp;
    public event Action<float> OnDecreaseMaxHp;
    public event Action<float> OnSetCurrentHp;
    public event Action<float> OnIncreaseCurrentHp;
    public event Action<float> OnDecreaseCurrentHp;

    public void OnGameStart()
    {
        SetMaxHp(maxHP);
        SetCurrentHp(maxHP);

        Debug.Log($"Starting Ship Core with {CurrentHP} hp");
    }

    private void CheckCurrentHPAmount()
    {
        if (CurrentHP <= 0) Die();
    }

    private void Die()
    {
        Debug.Log("Lost the game");

        GameManager.Instance.OnGameEnd(); 

        // set new state
    }

    public float GetMaxHp() => MaxHP;

    public float GetCurrentHp() => CurrentHP;

    public void SetMaxHp(float value)
    {
        MaxHP = value;
    }

    public void IncreaseMaxHp(float amount)
    {
        MaxHP += amount;
    }

    public void DecreaseMaxHp(float amount)
    {
        MaxHP -= amount;
    }

    public void SetCurrentHp(float value)
    {
        CurrentHP = value;
    }

    public void IncreaseCurrentHp(float amount)
    {
        CurrentHP += amount;
    }

    public void DecreaseCurrentHp(float amount)
    {
        CurrentHP -= amount;
        Debug.Log("Current hp: " + CurrentHP);

        CheckCurrentHPAmount();
    }
}
