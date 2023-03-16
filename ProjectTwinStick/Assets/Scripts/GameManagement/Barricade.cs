using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Barricade : MonoBehaviour, ILifeable
{
    [SerializeField, Range(0, 1000)] private float maxHP = 1f;

    public UnityEvent OnBarricadeDestroyed;
    public UnityEvent OnBarricadeDamaged;
    public UnityEvent OnBarricadeRepaired;
    [SerializeField] private BarricadeRenderer _barricadeRenderer;
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

    [SerializeField] AudioSource audioSourceRepair;
    [SerializeField] AudioSource audioSourceDestroy;

    private void Start()
    {
        audioSourceDestroy.volume = 0;
        DestroySelf();
    }

    public void OnGameStart()
    {
        _barricadeRenderer.Init();
        SetMaxHp(maxHP);
        SetCurrentHp(maxHP);
        audioSourceRepair.volume = 0;
        RepairSelf();
        Invoke("ResetSound", 3f);
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
        CurrentHP = Mathf.Clamp(CurrentHP, 0, MaxHP);
        Debug.Log($"Barricade {gameObject.name} max hp: {value}");
    }

    public void IncreaseMaxHp(float amount)
    {
        MaxHP += amount;
        CurrentHP = Mathf.Clamp(CurrentHP, 0, MaxHP);
        Debug.Log($"Barricade {gameObject.name} max hp: {MaxHP}");
    }

    public void DecreaseMaxHp(float amount)
    {
        MaxHP -= amount;
        CurrentHP = Mathf.Clamp(CurrentHP, 0, MaxHP);
        Debug.Log($"Barricade {gameObject.name} max hp: {MaxHP}");
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
            Debug.Log($"Barricade {gameObject.name} current hp: {CurrentHP} max hp: {MaxHP}");
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
            //Debug.Log($"Barricade {gameObject.name} current hp: {CurrentHP}");
        }
    }

    public void ResetSound()
    {
        audioSourceRepair.volume = 0.4f;
        audioSourceDestroy.volume = 0.4f;
    }
}
