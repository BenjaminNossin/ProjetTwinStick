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
    #region LD Metrics
    private readonly float floorY = -0.5f;
    private readonly float levelRadius = 10f;
    private readonly float xzBufferZone = 2f;
    private readonly float jumpHeight = 1f;
    #endregion

    private Transform cachedTransf; 
    private Vector3 targetPostFlat, selfPosFlat;

    private Vector3 jumpPoint; 
    private bool hasJumped; // change to state machines if needed

    private Vector3 moveDir;
    private Vector3 shipCorePos; 

    private void Start()
    {
        GameManager.Instance.OnGameOverCallBack += Die;
        shipCorePos = GameManager.Instance.ShipCoreObj.transform.position; 
    }

    public void Init(Vector3 assignedBarricadePos)
    {
        cachedTransf = transform;

        SetTargetPosition(assignedBarricadePos);
        SetSelfAndTargetPosFlat(floorY);
        SetNormalizedDirection(); 
        
        SetMaxHp(maxHP);
        SetCurrentHp(maxHP);  
    }

    void FixedUpdate()
    {
        Move();

        /* if (Vector3.Distance(selfPosFlat, targetPostFlat) <= levelRadius + xzBufferZone)
        {
            if (!hasJumped)
            {
                JumpToPoint();
            }
        } */
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

    private void SetTargetPosition(Vector3 posToReach)
    {
        targetPos = posToReach;
    }

    private void SetSelfAndTargetPosFlat(float flattenValue)
    {
        selfPosFlat = new Vector3(cachedTransf.position.x, flattenValue, cachedTransf.position.z);
        targetPostFlat = new Vector3(targetPos.x, flattenValue, targetPos.z);

        cachedTransf.position = selfPosFlat;
    }

    private void SetNormalizedDirection()
    {
        normalizedDirection = new Vector3(targetPos.x - cachedTransf.position.x, 0, targetPos.z - cachedTransf.position.z).normalized;
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
        Debug.DrawRay(cachedTransf.position, normalizedDirection * 3f, Color.red, Time.deltaTime);
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

        SetTargetPosition(shipCorePos);
        SetSelfAndTargetPosFlat(cachedTransf.position.y);
        SetNormalizedDirection(); 
    }

    #endregion
}
