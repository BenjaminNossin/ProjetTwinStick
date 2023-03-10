using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Barricade : MonoBehaviour, ILifeable
{
    [SerializeField, Range(0, 10)] private float maxHP = 1f;

    public UnityEvent OnBarricadeDestroyed;
    public UnityEvent OnBarricadeDamaged;
    public UnityEvent OnBarricadeRepaired;
    
    private bool IsDestroyed = false;

    [SerializeField] private List<GameObject> RepairedCollision = new List<GameObject>();
    [SerializeField] private List<GameObject> DestroyedCollision = new List<GameObject>();
    
    public float MaxHP { get; private set; }
    public float CurrentHP { get; private set; }

    public event Action<float> OnSetMaxHp;
    public event Action<float> OnIncreaseMaxHp;
    public event Action<float> OnDecreaseMaxHp;
    public event Action<float> OnSetCurrentHp;
    public event Action<float> OnIncreaseCurrentHp;
    public event Action<float> OnDecreaseCurrentHp;

    private void OnEnable()
    {
        
    }

    public void OnGameStart()
    {
        SetMaxHp(maxHP);
        SetCurrentHp(maxHP);
        for(int i = 0; i < RepairedCollision.Count; i++)
        {
            RepairedCollision[i].SetActive(true);
        }
        for(int i = 0; i < DestroyedCollision.Count; i++)
        {
            DestroyedCollision[i].SetActive(false);
        }
    }

    private void DestroySelf()
    {
        OnBarricadeDestroyed?.Invoke();
        for(int i = 0; i < RepairedCollision.Count; i++)
        {
            RepairedCollision[i].SetActive(false);
        }
        for(int i = 0; i < DestroyedCollision.Count; i++)
        {
            DestroyedCollision[i].SetActive(true);
        }
        IsDestroyed = true;
    }
    
    private void RepairSelf()
    {
        OnBarricadeRepaired?.Invoke();
        for(int i = 0; i < RepairedCollision.Count; i++)
        {
            RepairedCollision[i].SetActive(true);
        }
        for(int i = 0; i < DestroyedCollision.Count; i++)
        {
            DestroyedCollision[i].SetActive(false);
        }
        IsDestroyed = false;
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
        if (IsDestroyed)
        {
            CurrentHP += amount;
            if (CurrentHP >= MaxHP)
            {
                CurrentHP = MaxHP;
                RepairSelf();
            }
            Debug.Log($"Barricade {gameObject.name} current hp: {CurrentHP}");
        }
    }

    public void DecreaseCurrentHp(float amount)
    {
        if (!IsDestroyed)
        {
            CurrentHP -= amount;
            if (CurrentHP <= 0)
            {
                CurrentHP = 0;
                DestroySelf();
            }
            else OnBarricadeDamaged?.Invoke();
            Debug.Log($"Barricade {gameObject.name} current hp: {CurrentHP}");
        }
    }
}
