using Game.Systems.GlobalFramework;
using HelperPSR.Pool;
using System;
using UnityEngine;

// TODO: change to a state machine
// delay before jump
// delay before destroying shipcore
    // states: ShipCoreReaching, Jumping, ShipCoreDestroying

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
    private const float floorY = -1f;
    private const float levelRadius = 10f;
    private const float xzBufferZone = 2f;
    private const float jumpHeight = 1f;
    #endregion

    private Transform cachedTransf; 
    private Vector3 targetPostFlat, selfPosFlat;

    private bool canJump = true; 

    private Vector3 shipCorePosFlat;
    
    private SlowManager slowManager;


    // DEBUG
    public float distFromShipCore, distFromJumpArea; 

    private void Start()
    {
        slowManager = GetComponent<SlowManager>();
        GameManager.Instance.OnGameOverCallBack += Die;
        var shipCorePos = GameManager.Instance.ShipCoreObj.transform.position;
        shipCorePosFlat = new Vector3(shipCorePos.x, floorY, shipCorePos.z); 
    }

    public void Init(Vector3 assignedBarricadePos)
    {
        cachedTransf = transform;
        distFromJumpArea = levelRadius + xzBufferZone;

        SetTargetPosition(assignedBarricadePos);
        SetSelfAndTargetPosFlat(floorY);
        SetNormalizedDirection(); 
        
        SetMaxHp(maxHP);
        SetCurrentHp(maxHP);  
    }

    void Update()
    {
        Move();

        // state machine will avoid this kind of shitty checks
        if (canJump)
        {
            // this may be extracted + abstracted has a target check for both jump and final attack on the ship core
            // JumpToPoint becomes an abstract Act() -> Jump/Explode
            distFromShipCore = Vector3.Distance(cachedTransf.position, shipCorePosFlat);
            if (distFromShipCore <= distFromJumpArea)
            {
                Jump();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {   
        if (other.TryGetComponent<ILifeable>(out var lifeable))
        {
            //Debug.Log($"damaging target {other.gameObject.name}"); 
            DamageTarget(lifeable); 
            Die();
        }
        else
        {
            lifeable = other.GetComponentInParent<ILifeable>();
            if (lifeable != null)
            {
                //Debug.Log($"damaging target {other.gameObject.name}"); 
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
        Debug.DrawRay(cachedTransf.position, normalizedDirection * 3f, Color.red, Time.deltaTime);
        cachedTransf.Translate(Time.deltaTime * unitsPerSeconds * slowManager.GetCurrentSlowMultiplier() * normalizedDirection, Space.Self);
    }

    private void Jump()
    {
        
        cachedTransf.position += new Vector3(
                                        normalizedDirection.x * xzBufferZone,
                                        jumpHeight,
                                        normalizedDirection.z * xzBufferZone);

        canJump = false;

        SetTargetPosition(shipCorePosFlat);
        SetSelfAndTargetPosFlat(cachedTransf.position.y);
        SetNormalizedDirection(); 
    }

    #endregion
}
