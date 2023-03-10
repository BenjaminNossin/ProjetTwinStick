using System;
using UnityEngine;
using Game.Systems.GlobalFramework;
using System.Collections.Generic;

public class ShipCore : MonoBehaviour, ILifeable
{
    [SerializeField, Range(0, 200)] private float maxHP = 100f;
    List<Barricade> barricades = new(); // temp. 

    public float MaxHP { get; private set; }
    public float CurrentHP { get; private set; }

    public event Action<float> OnSetMaxHp;
    public event Action<float> OnIncreaseMaxHp;
    public event Action<float> OnDecreaseMaxHp;
    public event Action<float> OnSetCurrentHp;
    public event Action<float> OnIncreaseCurrentHp;
    public event Action<float> OnDecreaseCurrentHp;

    private void Start()
    {
        GameManager.Instance.SetShipCoreData(gameObject, this); 
        GameManager.Instance.SetObjectActive(gameObject, false);
    }

    public void OnGameStart()
    {
        SetMaxHp(maxHP);
        SetCurrentHp(maxHP);

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform currentTransf = transform.GetChild(i);
            if (currentTransf.gameObject.activeInHierarchy)
            {
                barricades.Add(currentTransf.GetComponent<Barricade>());
            }
        }

        InitActiveBarricades(); 
    }

    private void InitActiveBarricades()
    {
        barricades.Clear();
        GetComponentsInChildren(barricades);
        
        foreach (var item in barricades)
        {
            item.OnGameStart(); 
        }
    }

    private void CheckCurrentHPAmount()
    {
        if (CurrentHP <= 0) Die();
    }

    private void Die()
    {
        Debug.Log("Lost the game");

        GameManager.Instance.OnGameEnd(); 
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

        CheckCurrentHPAmount();
    }
}
