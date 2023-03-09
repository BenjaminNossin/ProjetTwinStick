using System;
using UnityEngine;

public class Barricade : MonoBehaviour, ILifeable
{
    [SerializeField, Range(0, 10)] private float maxHP = 1f;
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
    }

    private void CheckCurrentHPAmount()
    {
        if (CurrentHP <= 0) DestroySelf();
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
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
        if (CurrentHP >= MaxHP) CurrentHP = MaxHP;
        Debug.Log($"Barricade {gameObject.name} current hp: {CurrentHP}");
    }

    public void IncreaseCurrentHp(float amount)
    {
        CurrentHP += amount;
        if (CurrentHP >= MaxHP) CurrentHP = MaxHP;
        Debug.Log($"Barricade {gameObject.name} current hp: {CurrentHP}");
    }

    public void DecreaseCurrentHp(float amount)
    {
        CurrentHP -= amount;
        if (CurrentHP >= MaxHP) CurrentHP = MaxHP;
        Debug.Log($"Barricade {gameObject.name} current hp: {CurrentHP}");

        CheckCurrentHPAmount();
    }
}
