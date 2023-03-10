using Game.Systems.GlobalFramework;
using HelperPSR.Pool;
using System;
using UnityEngine;

public class BasicAI : MonoBehaviour, ILifeable
{
    [SerializeField, Range(0, 20)] private float maxHP = 10f;  
    [SerializeField, Range(1, 20)] float unitsPerSeconds = 10;
    [SerializeField, Range(1, 20)] float damage = 20f;

    [SerializeField] 
    private Vector3 normalizedDirection;

    public Pool<BasicAI> _pool; 
    public float MaxHP { get; private set; }    
    public float CurrentHP { get; private set; }

    public event Action<float> OnSetMaxHp;
    public event Action<float> OnIncreaseMaxHp;
    public event Action<float> OnDecreaseMaxHp;
    public event Action<float> OnSetCurrentHp;
    public event Action<float> OnIncreaseCurrentHp;
    public event Action<float> OnDecreaseCurrentHp;

    private Vector3 targetPos;
    private readonly float levelRadius = 10f;
    private readonly float xzBufferZone = 2f;
    private readonly float jumpHeight = 1f;

    private Transform cachedTransf; 
    private Vector3 targetPostFlat, selfPosFlat;

    private Vector3 jumpPoint; 
    private bool hasJumped; // change to state machines if needed

    private Vector3 moveDir;

    private void Start()
    {
        GameManager.Instance.OnGameOverCallBack += Die;
    }

    public void Init(Vector3 assignedBarricadePos)
    {
        cachedTransf = transform; 

        targetPos = assignedBarricadePos;
        targetPostFlat = new Vector3(targetPos.x, 0f, targetPos.z); 

        normalizedDirection = new Vector3(targetPos.x - transform.position.x, 0, targetPos.z - transform.position.z).normalized;
        
        SetMaxHp(maxHP);
        SetCurrentHp(maxHP);  
    }

    void FixedUpdate()
    {
        Move();

        if (!hasJumped)
        {
            selfPosFlat = new Vector3(cachedTransf.position.x, 0f, cachedTransf.position.z);
            if (Vector3.Distance(selfPosFlat, targetPostFlat) <= levelRadius + xzBufferZone)
            {
                JumpToPoint();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {   
        ILifeable lifeable = other.GetComponent<ILifeable>();
        if (lifeable != null)
        {
            Debug.Log($"damaging target {other.gameObject.name}"); 
            DamageTarget(lifeable); 
            Die();
        }
        else
        {
            lifeable = other.GetComponentInParent<ILifeable>();
            if (lifeable != null)
            {
                Debug.Log($"damaging target {other.gameObject.name}"); 
                DamageTarget(lifeable); 
                Die();
            }
        }
    }

    #region Life Management
    private void CheckCurrentHPAmount()
    {
        if (CurrentHP <= 0) Die();
    }

    private void Die()
    {
        _pool.AddToPool(this);
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
    #endregion

    #region Attack Behavior
    private void DamageTarget(ILifeable lifeable)
    {
        lifeable.DecreaseCurrentHp(damage);
    }

    #endregion

    #region Movement Behavior
    private void Move()
    {
        moveDir = normalizedDirection; 
        Debug.DrawRay(cachedTransf.position, normalizedDirection * 2f, Color.red, Time.deltaTime);
        cachedTransf.Translate(Time.deltaTime * unitsPerSeconds * normalizedDirection, Space.Self);
    }

    private void JumpToPoint()
    {
        Debug.Log("Jumping");
        
        cachedTransf.position += new Vector3(
                                        normalizedDirection.x * xzBufferZone,
                                        jumpHeight,
                                        normalizedDirection.z * xzBufferZone);

        hasJumped = true; 
    }

    #endregion
}
