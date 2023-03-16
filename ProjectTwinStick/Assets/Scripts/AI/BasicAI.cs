using Game.Systems.GlobalFramework;
using HelperPSR.Pool;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// TODO: change to a state machine
// delay before jump
// delay before destroying shipcore
    // states: ShipCoreReaching, Jumping, ShipCoreDestroying

public class BasicAI : MonoBehaviour, ILifeable
{
    [FormerlySerializedAs("JumpTime")] [SerializeField] private float jumpTime = 0.75f;
    [FormerlySerializedAs("JumpApexHeight")] [SerializeField] private float jumpApexHeight = 3f;
    [FormerlySerializedAs("JumpCurve")] [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private LayerMask BarricadeMask;
    [SerializeField] private Collider collider;
    [SerializeField]
    private float dieTime;
    [SerializeField] 
    private Vector3 normalizedDirection;

    public Pool<BasicAI> _pool; 
    public float MaxHP { get; private set; }    
    public float CurrentHP { get; private set; }

    public event Action OnDieImmedialty;
    public event Action OnHit;
    public event Action OnDieByPlayer;
    public event Action<Vector3> OnSetMoveDirection;
    public event Action<float> OnSetMaxHp;
    public event Action<float> OnIncreaseMaxHp;
    public event Action<float> OnDecreaseMaxHp;
    public event Action<float> OnSetCurrentHp;
    public event Action<float> OnIncreaseCurrentHp;
    public event Action<float> OnDecreaseCurrentHp;

    public UnityEvent OnStartRun;
    public UnityEvent OnStartJump;

    private Vector3 targetPos;

    #region LD Metrics
    private const float floorY = -1f;
    private const float levelRadius = 10f;
    private const float xzBufferZone = 2f;
    private const float jumpHeightTarget = 1f;
    #endregion

    private Transform cachedTransf; 
    private Vector3 targetPostFlat, selfPosFlat;

    private bool canJump = true; 

    private Vector3 shipCorePosFlat;
    
    private SlowManager slowManager;

    private EnemyStats currentStats;

    private enum BasicAIState
    {
        Run,
        Jump,
        Dead
    }
    
    private BasicAIState currentState = BasicAIState.Run;

    private float currentJumpTime = 0f;

    private Vector3 jumpStartPos;
    private Vector3 jumpEndPos;


    [SerializeField]
    private BasicAIRender _render; 
    // DEBUG
    public float distFromShipCore, distFromJumpArea;

    private void Awake()
    {
        _render.Init();
    }

    private void Start()
    {
        slowManager = GetComponent<SlowManager>();
        GameManager.Instance.OnGameOverCallBack += ResetEnemyWhenGameOver;
        var shipCorePos = GameManager.Instance.ShipCoreObj.transform.position;
        shipCorePosFlat = new Vector3(shipCorePos.x, floorY, shipCorePos.z); 
       
    }

    public void Init(Vector3 assignedBarricadePos, EnemyStats stats)
    {
        currentStats = stats;
        cachedTransf = transform;
        distFromJumpArea = levelRadius + xzBufferZone;

        SwitchState(BasicAIState.Run);
        collider.enabled = true;
        SetTargetPosition(assignedBarricadePos);
        SetSelfAndTargetPosFlat(floorY);
        SetNormalizedDirection(); 
        
        SetMaxHp(currentStats.maxHP);
        SetCurrentHp(currentStats.maxHP);
        canJump = true;
    }
    
    private void SwitchState(BasicAIState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case BasicAIState.Run:
                OnStartRun?.Invoke();
                break;
            case BasicAIState.Jump:
                OnStartJump?.Invoke();
                break;
            case BasicAIState.Dead:
                break;
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case BasicAIState.Jump:
                JumpUpdate();
                break;
            case BasicAIState.Run:
                RunUpdate();
                break;
        }
    }

    private void RunUpdate()
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
                if (TryDamageBarricade())
                {
                    return;
                }
                currentJumpTime = 0;
                jumpStartPos = cachedTransf.position;
                jumpEndPos = cachedTransf.position + new Vector3(
                    normalizedDirection.x * xzBufferZone,
                    jumpHeightTarget,
                    normalizedDirection.z * xzBufferZone);
                SwitchState(BasicAIState.Jump);
            }
        }
    }

    private void ResetEnemyWhenGameOver()
    {
        _pool.AddToPool(this);
        collider.enabled = false;
        SwitchState(BasicAIState.Dead);
    }
    private void JumpUpdate()
    {
        canJump = false;
        currentJumpTime += Time.deltaTime;
        
        
        float horizontalPosLerpValue = currentJumpTime / jumpTime;
        float verticalPosLerpValue = jumpCurve.Evaluate(horizontalPosLerpValue);
        
        Vector3 currentHorizontalPos = Vector3.Lerp(jumpStartPos, jumpEndPos, horizontalPosLerpValue);
        float height = Mathf.Lerp(currentHorizontalPos.y, jumpApexHeight, verticalPosLerpValue);
        currentHorizontalPos.y = height;

        cachedTransf.position = currentHorizontalPos;
        
        if (currentJumpTime >= jumpTime)
        {
            SetTargetPosition(shipCorePosFlat);
            SetSelfAndTargetPosFlat(cachedTransf.position.y);
            SetNormalizedDirection(); 
            SwitchState(BasicAIState.Run);
            currentJumpTime = 0f;
        }
        
        /*
        cachedTransf.position += new Vector3(
            normalizedDirection.x * xzBufferZone,
            jumpHeight,
            normalizedDirection.z * xzBufferZone);
        SetTargetPosition(shipCorePosFlat);
        SetSelfAndTargetPosFlat(cachedTransf.position.y);
        SetNormalizedDirection(); 
        SwitchState(BasicAIState.Run);
        */
    }

    private bool TryDamageBarricade()
    {
        Collider[] colliders = Physics.OverlapSphere(targetPos, 0.5f, BarricadeMask, QueryTriggerInteraction.Collide);
        if (colliders.Length > 0)
        {
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<ILifeable>(out var lifeable))
                {
                    //Debug.Log($"damaging target {other.gameObject.name}"); 
                    DamageTarget(lifeable); 
                    DieImmediately();
                }
                else
                {
                    lifeable = collider.GetComponentInParent<ILifeable>();
                    if (lifeable != null)
                    {
                        //Debug.Log($"damaging target {other.gameObject.name}"); 
                        DamageTarget(lifeable); 
                        DieImmediately();
                    }
                }
            }
        }

        return false;

    }

    private void DieImmediately()
    {
       OnDieImmedialty?.Invoke();
        _pool.AddToPool(this);
        collider.enabled = false;
        SwitchState(BasicAIState.Dead);
    }

    private void OnTriggerEnter(Collider other)
    {   
        if (other.TryGetComponent<ILifeable>(out var lifeable))
        {
            //Debug.Log($"damaging target {other.gameObject.name}"); 
            DamageTarget(lifeable); 
            DieImmediately();
        }
        else
        {
            lifeable = other.GetComponentInParent<ILifeable>();
            if (lifeable != null)
            {
                //Debug.Log($"damaging target {other.gameObject.name}"); 
                DamageTarget(lifeable); 
                DieImmediately();
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
        OnSetMoveDirection?.Invoke(normalizedDirection);

    }

    #region Life Management
    private void CheckCurrentHPAmount()
    {
        if (CurrentHP <= 0)
        {
            OnDieByPlayer?.Invoke();
            Die();
        }
        else OnHit?.Invoke();
    }

    private void Die()
    {
        
        StartCoroutine(_pool.AddToPoolLatter(this, dieTime));
        SwitchState(BasicAIState.Dead);
        collider.enabled = false;
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
        if(currentState == BasicAIState.Dead) return;
        CurrentHP -= amount;
        CheckCurrentHPAmount(); 
       
    }
    #endregion

    #region Attack Behavior
    private void DamageTarget(ILifeable lifeable)
    {
        lifeable.DecreaseCurrentHp(currentStats.damage);
    }

    #endregion

    #region Movement Behavior
    private void Move()
    {
        Debug.DrawRay(cachedTransf.position, normalizedDirection * 3f, Color.red, Time.deltaTime);
        cachedTransf.Translate(Time.deltaTime * currentStats.unitsPerSeconds * slowManager.GetCurrentSlowMultiplier() * normalizedDirection, Space.Self);
    }


    #endregion
}
