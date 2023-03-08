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


    private int _upgradeCount;

    private GameObject _itemHolder;
    private float _throwLength = 1f;

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
    }

    private void OnThrowStart()
    {
        _throwTimer = 0;
        
        Vector3 position = _itemHolder.transform.position;
        transform.parent = null;
        _itemHolder = null;
        transform.position = new Vector3(position.x, 1, position.z);
        transform.rotation = Quaternion.LookRotation(MovementDirection, Vector3.up);
        MovementStartPosition = transform.position;
    }

    private void OnBounceStart()
    {
        _bounceTimer = 0;
    }

    private void OnHeld()
    {
        
        transform.parent = _itemHolder.transform;
    }
    
    private void OnDropped()
    {
        Vector3 position = transform.position;
        if (_itemHolder != null)
        {
            position = _itemHolder.transform.position;
        }
        transform.parent = null;
        _itemHolder = null;
        Debug.Log(position);
        transform.position = new Vector3(position.x, 1, position.z);
        transform.rotation = quaternion.identity;
    }

    public abstract ItemSO GetSO();
    public abstract void Shoot(Vector3 startPosition, Vector2 direction);

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
        }
    }

    private void ThrowUpdate()
    {
        _throwTimer = Mathf.MoveTowards(_throwTimer,_throwLength, Time.deltaTime * throwData.ThrowSpeed);
        Vector3 NextPos = MovementStartPosition + MovementDirection * _throwTimer;

        RaycastHit hit;
        if (Physics.SphereCast(transform.position, _collider.radius, MovementDirection, out hit,throwData.ThrowSpeed * Time.deltaTime,
                throwData.BlockerMask))
        {
            ChangeState(ItemState.Dropped);
            return;
        }
        
        float heightmask = throwData.ThrowCurve.Evaluate(_throwTimer / _throwLength);
        NextPos.y = heightmask * throwData.ThrowHeight;
        transform.position = NextPos;
        if (_throwTimer == _throwLength)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _collider.radius, throwData.PlayerMask, QueryTriggerInteraction.Collide);
            Debug.Log(colliders);
            if (colliders.Length > 0)
            {
                Debug.Log("Item catch");
                Upgrade();
                Inventory _inventory = colliders[0].GetComponentInParent<Inventory>();
                _inventory.SetItem(this);
                Take(_inventory.gameObject);
                return;
            }
            
            ChangeState(ItemState.Dropped);
            ResetUpgrade();
        }

    }

    private void BounceUpdate()
    {
        
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

    public virtual void Throw(float throwForce, Vector3 direction)
    {
        if (CurrentItemState == ItemState.Held)
        {
            Debug.Log("Thrown");
            _throwLength = throwForce;
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

    public void Take(GameObject holder)
    {
        _itemHolder = holder;
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

    void UpdateUpgrade() => SetUpgrade(GetSO().GetUpgrades()[_upgradeCount]);

    public void ResetUpgrade()
    {
        _upgradeCount = 0;
        UpdateUpgrade();
    }

}