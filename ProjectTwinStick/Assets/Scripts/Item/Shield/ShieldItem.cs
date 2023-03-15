using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShieldItem : Item
{
    
    [SerializeField] ShieldItemSO _shieldItemSO;
    [SerializeField] ShieldInstance _shieldInstance;
    private ShieldItemUpgrade _currentUpgrade;
    private ShieldCenter _shieldCenter;

    private Vector3 lastStartPos;
    private Vector2 lastDirection;

    private bool IsInUse = false;
    private bool IsInOppositeCorner = false;
    
    
    public override ItemSO GetSO()
    {
        return _shieldItemSO;
    }

    protected override void Start()
    {
        OnItemStateChange += OnItemStateChanged;
        SetUpgrade(_shieldItemSO.GetUpgrades()[0]);
        _shieldCenter = FindObjectOfType<ShieldCenter>();
        _shieldInstance.transform.parent = null;
    }

    protected override void Update()
    {
        base.Update();
        UpdateShieldPos();
    }

    private void OnItemStateChanged(ItemState state)
    {
        if(state != ItemState.Held)
        {
            IsInUse = false;
        }
        else IsInUse = true;
    }

    private void UpdateShieldState()
    {
        if (IsInUse && !IsInOppositeCorner)
        {
            EnableShield();
        }
        else DisableShield();
    }

    private void DisableShield()
    {
        _shieldInstance.stopUsing();
    }

    private void EnableShield()
    {
        _shieldInstance.startUsing();
    }

    //TODO : use center gameobject for center and radius
    private Vector3 GetIntersectionPoint(Vector3 startPosition, Vector2 direction)
    {
        Vector2 circleCenter = new Vector2(_shieldCenter.transform.position.x, _shieldCenter.transform.position.z);
        float circleRadius = _shieldCenter.Radius;
        Vector2 rayOrigin = new Vector2(startPosition.x, startPosition.z);
        Vector2 rayDirection = direction.normalized;
        
        Vector2 U = circleCenter - rayOrigin;
        Vector2 U1 = Vector2.Dot(U,rayDirection) * rayDirection;
        
        Vector2 U2 = U - U1;

        float d = U2.magnitude;
        
        float m = Mathf.Sqrt(circleRadius * circleRadius - U2.magnitude * U2.magnitude);
        
        Vector2 P1 = rayOrigin + U1 + m * rayDirection;
        Vector2 P2 = rayOrigin + U1 - m * rayDirection;
        
        return new Vector3(P1.x, 0, P1.y);
    }

    public override bool TryShoot(Vector3 startPosition, Vector2 direction)
    {
        lastDirection = direction;
        lastStartPos = _itemHolder.transform.position;
        OnShoot?.Invoke();
        return true;
    }

    private void UpdateShieldPos()
    {
        Vector3 intersectionPoint = GetIntersectionPoint(lastStartPos, lastDirection);
        intersectionPoint.y = _shieldItemSO.ShieldHeight;

        Vector3 relativeIntersectionPoint = _shieldCenter.transform.InverseTransformPoint(intersectionPoint);
        Vector3 corner = _shieldCenter.transform.InverseTransformPoint(lastStartPos).normalized;
        
        corner.x = Mathf.Sign(corner.x);
        corner.y = _shieldItemSO.ShieldHeight;
        corner.z = Mathf.Sign(corner.z);
        
        /*
        //if shield in wrong corner
        if (Math.Abs(Mathf.Sign(relativeIntersectionPoint.x) - corner.x) > 0.0001 || Math.Abs(Mathf.Sign(relativeIntersectionPoint.z) - corner.z) > 0.0001)
        {
            IsInOppositeCorner = false;
        }
        else IsInOppositeCorner = true;*/
        
        relativeIntersectionPoint.y = 0;

        if (corner.x < 0 && relativeIntersectionPoint.x > 0)
        {
            relativeIntersectionPoint.x = 0;
        }
        else if (corner.x > 0 && relativeIntersectionPoint.x < 0)
        {
            relativeIntersectionPoint.x = 0;
        }
        if (corner.z < 0 && relativeIntersectionPoint.z > 0)
        {
            relativeIntersectionPoint.z = 0;
        }
        else if (corner.z > 0 && relativeIntersectionPoint.z < 0)
        {
            relativeIntersectionPoint.z = 0;
        }

        relativeIntersectionPoint.Normalize();
        if (relativeIntersectionPoint.magnitude < 0.99f)
        {
            IsInOppositeCorner = true;
            UpdateShieldState();
            return;
        }
        else
        {
            IsInOppositeCorner = false;
            UpdateShieldState();
        }
        
        relativeIntersectionPoint *= _shieldCenter.Radius;
        intersectionPoint = _shieldCenter.transform.TransformPoint(relativeIntersectionPoint);
        intersectionPoint.y = _shieldItemSO.ShieldHeight;
        
        //TODO : Raycast to edge of map instead
        lastDirection.Normalize();
        Vector3 mapCenter = _shieldCenter.transform.position;
        mapCenter.y = _shieldItemSO.ShieldHeight;
        Vector3 directionToCenter = mapCenter - intersectionPoint;
        directionToCenter.Normalize();
        
        _shieldInstance.transform.position = intersectionPoint;
        _shieldInstance.transform.rotation = Quaternion.LookRotation(directionToCenter, Vector3.up);
    }

    
    public override void SetUpgrade(ItemUpgrade newUpgrade)
    {
        _currentUpgrade = (ShieldItemUpgrade) newUpgrade;
        _shieldInstance.ChangeUpgrade(_currentUpgrade);
    }

    private void OnDrawGizmos()
    {
        if (_shieldCenter == null)
        {
            _shieldCenter = FindObjectOfType<ShieldCenter>();
        };
        Gizmos.color = Color.green;
        Vector3 intersectionPoint = GetIntersectionPoint(lastStartPos, lastDirection);
        intersectionPoint.y = lastStartPos.y;
        Gizmos.DrawLine(lastStartPos, intersectionPoint);
    }
}
