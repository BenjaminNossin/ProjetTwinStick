using HelperPSR.Pool;
using System;
using UnityEngine;

public class BasicAI : MonoBehaviour, ILifeable
{
    [SerializeField, Range(0, 20)] private float maxHP = 10f;  
    [SerializeField, Range(1, 20)] float unitsPerSeconds = 10;
    [SerializeField, Range(1, 20)] float damage = 20f;

    private Vector3 direction;

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
        direction = (GameObject.Find("Ship Core").transform.position - transform.position).normalized;
        SetMaxHp(maxHP);
        SetCurrentHp(maxHP); 
    }

    void FixedUpdate()
    {
        transform.Translate(Time.deltaTime * unitsPerSeconds * direction, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ShipCore"))
        {
            var lifeable = other.GetComponent<ILifeable>();
            lifeable?.DecreaseCurrentHp(damage);
            Die();
        }
    }

    private void CheckCurrentHPAmount()
    {
        if (CurrentHP <= 0) Die(); 
    }

    private void Die()
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
