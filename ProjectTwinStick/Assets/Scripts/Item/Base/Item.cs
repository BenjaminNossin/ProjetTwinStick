using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public enum ItemState
{
    Held,
    Thrown,
    Bouncing,
    Dropped
}

public abstract class Item : MonoBehaviour, IShootable, IDropable, ITakeable, IThrowable, IUpgradable
{
    private const int upgradeMaxCount = 3;

    [SerializeField] private float GroundedHeight = 1f;

    [SerializeField] ItemThrowData throwData;

    [SerializeField] private ItemSpawner _itemSpawner;

     public Transform handPivotPoint;
     public Transform shootPivotPoint;
    public event Action<ItemState> OnItemStateChange;

    public event Action<int>  OnItemUpgradeChanged;
  


    private int _upgradeCount;
    private bool isInRespawn;
    private bool isInSpawner;
    public int UpgradeCount
    {
        get => _upgradeCount;
    }

    private float upgradeResetTimer = 0f;

    private ItemsSpawnerManager _itemsSpawnerManager;
    public event Action OnSpawn;
  
    public void Init(ItemsSpawnerManager itemsSpawnerManager)
    {
        isInRespawn = false;
        isInSpawner = false;

        StopAllCoroutines();

        _itemsSpawnerManager = itemsSpawnerManager;
        ChangeState(ItemState.Dropped);
    }

    public void CheckMaxDistanceOfArena()
    {
        if(isInRespawn || !_itemSpawner) return;
            if (Vector3.Distance(_itemsSpawnerManager.centerArena.position, new Vector3(transform.position.x, 0, transform.position.z))> _itemsSpawnerManager.maxDistanceToCenterOfArena)
            {
                isInRespawn = true;
                StartCoroutine(WaitForSpawn());
            }
    }

    IEnumerator WaitForSpawn()
    {
        yield return new WaitForSeconds(GetSO().RespawnTime);
        OnSpawn?.Invoke();
        isInRespawn = false;
        isInSpawner = true;
        _itemSpawner = null;
        _itemsSpawnerManager.Spawn(this);

    }
    public void SetItemSpawner(ItemSpawner spawner)
    {
        _itemSpawner = spawner;
        isInSpawner = true;
    }

    protected GameObject _previousHolder { get; private set; }
    protected GameObject _itemHolder { get; private set; }
    private float _chargeTime = 1f;

    private float _throwTimer = 0f;
    private float _bounceTimer = 0f;

    private Vector3 MovementStartPosition;
    private Vector3 MovementDirection;

    private SphereCollider _collider;

    protected ItemState CurrentItemState { get; private set; } = ItemState.Dropped;

    private void ChangeState(ItemState newState)
    {
        CurrentItemState = newState;
        Debug.Log("Current state : " + CurrentItemState);
        switch (CurrentItemState)
        {
            case ItemState.Held:
                OnHeld();
                break;
            case ItemState.Thrown:
                OnThrowStart();
                break;
            case ItemState.Bouncing:
                OnBounceStart();
                break;
            case ItemState.Dropped:
                OnDropped();
                break;
        }
        OnItemStateChange?.Invoke(CurrentItemState);
        Debug.Log("Current state : " + CurrentItemState);
    }

    private void OnThrowStart()
    {
        _throwTimer = 0;
        
        Vector3 position = _itemHolder.transform.position;
        transform.parent = null;
        _previousHolder = _itemHolder;
        _itemHolder = null;
        transform.position = new Vector3(position.x, throwData.GroundedHeight, position.z);
        transform.rotation = Quaternion.LookRotation(MovementDirection, Vector3.up);
        MovementStartPosition = transform.position;
    }

    private void OnBounceStart()
    {
        _bounceTimer = 0;
    }

    private void OnHeld()
    {
        upgradeResetTimer = GetSO().UpgradeResetTimer;
        if (isInSpawner)
        {
            _itemsSpawnerManager.AddSpawnerAvailable(_itemSpawner);
            isInSpawner = false; 
        }

    }
    
    private void OnDropped()
    {
        Vector3 position = transform.position;
        if (_itemHolder != null)
        {
            position = _itemHolder.transform.position;
            _previousHolder = _itemHolder;
        }
        _itemHolder = null;
        transform.position = new Vector3(position.x, throwData.GroundedHeight, position.z);
        transform.rotation = quaternion.identity;
        CheckMaxDistanceOfArena();
    }

    public abstract ItemSO GetSO();
    public abstract bool TryShoot(Vector3 startPosition, Vector2 direction);
    public Action OnShoot;

    public Action OnUnShoot;
    public void CancelShoot()
    {
        OnUnShoot?.Invoke();
    }
    public abstract void SetUpgrade(ItemUpgrade newUpgrade);

    protected void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    protected virtual void Start()
    {
       ResetUpgrade();
    }

    protected virtual void Update()
    {
        switch (CurrentItemState)
        {
            case ItemState.Thrown:
                ThrowUpdate();
                break;
            case ItemState.Bouncing:
                BounceUpdate();
                break;
            case ItemState.Held:
                HeldUpdate();
                break;
        }
    }

    private void HeldUpdate()
    {
        if (upgradeResetTimer >= 0 && _upgradeCount > 0)
        {
            upgradeResetTimer -= Time.deltaTime;
            if (upgradeResetTimer <= 0)
            {
                Debug.Log("downgrading");
                Downgrade();
            }
        }
        else
        {
            upgradeResetTimer = GetSO().UpgradeResetTimer;
        }
    }

    private void ThrowUpdate()
    {
        _throwTimer = Mathf.MoveTowards(_throwTimer,throwData.GetThrowDistance(_chargeTime), Time.deltaTime * throwData.ThrowSpeed);
        Vector3 NextPos = MovementStartPosition + MovementDirection * _throwTimer;

        RaycastHit hit;
        if (Physics.SphereCast(transform.position, _collider.radius, MovementDirection, out hit,throwData.ThrowSpeed * Time.deltaTime,
                throwData.BlockerMask))
        {
            Vector3 flatnormal = hit.normal;
            flatnormal.y = 0;
            flatnormal.Normalize();
            Vector3 reflect = Vector3.Reflect(MovementDirection, flatnormal);
            Bounce(reflect);
            return;
        }
        
        float heightmask = throwData.ThrowCurve.Evaluate(_throwTimer / throwData.GetThrowDistance(_chargeTime));
        NextPos.y = heightmask * throwData.ThrowHeight;
        transform.position = NextPos;
        if (Math.Abs(_throwTimer - throwData.GetThrowDistance(_chargeTime)) < 0.001f)
        {
            //checking for player
            Collider[] colliders = Physics.OverlapSphere(transform.position, _collider.radius, throwData.PlayerMask, QueryTriggerInteraction.Collide);
            if (colliders.Length > 0)
            {
                Debug.Log("Item catch");
                if (colliders[0].gameObject != _previousHolder)
                {
                    Upgrade();
                }
                Inventory _inventory = colliders[0].GetComponentInParent<Inventory>();
                _inventory.SetItem(this);
                Take(_inventory.gameObject);
                return;
            }
            
            //checking for wall
            colliders = Physics.OverlapSphere(transform.position, _collider.radius, throwData.BlockerMask, QueryTriggerInteraction.Collide);
            if (colliders.Length > 0)
            {
                Bounce(MovementDirection);
                return;
            }
            
            ChangeState(ItemState.Dropped);
            ResetUpgrade();
        }

    }

    private void BounceUpdate()
    {
        _bounceTimer = Mathf.MoveTowards(_bounceTimer, throwData.GetBounceDistance(_chargeTime), Time.deltaTime * throwData.BounceSpeed);
        Vector3 NextPos = MovementStartPosition + MovementDirection * _bounceTimer;
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, _collider.radius, MovementDirection, out hit,throwData.ThrowSpeed * Time.deltaTime,
                throwData.BlockerMask))
        {
            Vector3 flatnormal = hit.normal;
            flatnormal.y = 0;
            flatnormal.Normalize();
            Vector3 reflect = Vector3.Reflect(MovementDirection, flatnormal);
            Bounce(reflect);
            return;
        }
        float heightmask = throwData.BounceCurve.Evaluate(_bounceTimer / throwData.GetBounceDistance(_chargeTime));
        NextPos.y = heightmask * throwData.BounceHeight;
        transform.position = NextPos;
        if (Math.Abs(_bounceTimer - throwData.GetBounceDistance(_chargeTime)) < 0.001f)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _collider.radius, throwData.PlayerMask, QueryTriggerInteraction.Collide);
            if (colliders.Length > 0)
            {
                Debug.Log("Item catch");
                if (colliders[0].gameObject != _previousHolder)
                {
                    Upgrade();
                }
                Inventory _inventory = colliders[0].GetComponentInParent<Inventory>();
                _inventory.SetItem(this);
                Take(_inventory.gameObject);
                return;
            }
            
            //checking for wall
            colliders = Physics.OverlapSphere(transform.position, _collider.radius, throwData.BlockerMask, QueryTriggerInteraction.Collide);
            if (colliders.Length > 0)
            {
                Bounce(MovementDirection);
                return;
            }
            
            ChangeState(ItemState.Dropped);
            ResetUpgrade();
        }
    }

    public virtual bool CanDrop()
    {
        return true;
    }

    public virtual void Drop()
    {
        ChangeState(ItemState.Dropped);
        ResetUpgrade();
    }

    private void Bounce(Vector3 direction)
    {
        ResetUpgrade();
        Debug.Log("Bounce " + direction);
        direction.y = 0;
        direction.Normalize();
        transform.position = new Vector3(transform.position.x, throwData.GroundedHeight, transform.position.z);
        MovementStartPosition = transform.position;
        MovementDirection = direction;
        ChangeState(ItemState.Bouncing);
    }

    public virtual void Throw(float chargeTime, Vector3 direction)
    {
        if (CurrentItemState == ItemState.Held)
        {
            Debug.Log("Thrown");
            _chargeTime = chargeTime;
            direction.y = 0;
            direction.Normalize();
            MovementDirection = direction;
            ChangeState(ItemState.Thrown);
        }
        //Drop();
    }

    public virtual bool CanTake()
    {
        if (CurrentItemState == ItemState.Dropped)
        {
            return true;
        }

        return false;
    }

    public void Take( GameObject itemHolder)
    {
        if (_itemHolder != null)
        {
            _previousHolder = _itemHolder;
        }
        _itemHolder = itemHolder;
        ChangeState(ItemState.Held);
    }

    public void Upgrade()
    {
        if (_upgradeCount != upgradeMaxCount)
        {
            _upgradeCount++;
            
            UpdateUpgrade();
        }
    }

    public void Downgrade()
    {
        if (_upgradeCount != 0)
        {
            _upgradeCount--;
            UpdateUpgrade();
        }
    }

    void UpdateUpgrade()
    {
        OnItemUpgradeChanged?.Invoke(_upgradeCount);
        SetUpgrade(GetSO().GetUpgrades()[_upgradeCount]);
    }

    public void ResetUpgrade()
    {
        Debug.Log("Resetting upgrades");
        _upgradeCount = 0;
        UpdateUpgrade();
    }

}