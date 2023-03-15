using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerRenderer : MonoBehaviour
{
    public Animator animator;
    public Transform handTransform;
    public Transform pivotRenderer;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerShoot _playerShoot;

    [SerializeField] private SlowManager _slowManager;
    [SerializeField] private PlayerThrow _playerThrow;
    [SerializeField] private Inventory _inventory;

    [SerializeField] private GameObject _firstUpgradeFX;
    [SerializeField] private GameObject _secondUpgradeFX;
    [SerializeField] private GameObject _takedItemFX;
    public void Init()
    {
        _playerMovement.characterMovement.OnSetVelocity += SetBlendTreeMovement;
        _playerShoot.OnTryShoot += SetIsShoot;
        _playerShoot.OnCancelShoot += DisableIsShoot;
        _playerThrow.OnThrow += DisableIsThrow;
        _playerThrow.OnAim += EnableIsBeginThrow;
        _inventory.OnItemChanged += SnapItemTakedToHand;
        _inventory.OnItemDroped += UnSnapItem;
        _inventory.OnSetDefaultItem += ActivateDefaultItem;
        _slowManager.OnSlowAdded += EnableHitAnimationParameter;
        _slowManager.OnSlowRemove += TryDisableHitAnimationParameter;
        _playerThrow.OnCancelThrow += CancelThrowAnimation;
    }


    private void CancelThrowAnimation()
    {
        DisableIsThrow(); 
        animator.Play("Blend Tree");
    }
    private void UpdateUpgradeFX(Item currentItem)
    {
        switch (currentItem.UpgradeCount)
        {
            case 0:
            {
                _firstUpgradeFX.SetActive(false);
                _secondUpgradeFX.SetActive(false);
                break;
            }
            case 1:
            {
                _firstUpgradeFX.SetActive(true);
                break;
            }
            case 2:
            {
                _secondUpgradeFX.SetActive(true);
                break;
            }
        }
    }

    private void ActivateDefaultItem()
    {
        SnapItemToHand(_inventory.DefaultItem);
        _inventory.DefaultItem.gameObject.SetActive(true);
    }

    private void SnapItemTakedToHand(Item item)
    {
        SnapItemToHand(item);
        _inventory.DefaultItem.gameObject.SetActive(false);
        _takedItemFX.SetActive(true);
    }

    private void SnapItemToHand(Item item)
    {
        var itemTransform = item.transform;
        itemTransform.parent = handTransform;
        itemTransform.localPosition = Vector3.zero;
        itemTransform.localRotation = Quaternion.identity;
        if(item.handPivotPoint)
        itemTransform.position += itemTransform.transform.position - item.handPivotPoint.position;
        UpdateUpgradeFX(item);
    }


    private void UnSnapItem(Item item) => item.transform.parent = null;

    void EnableHitAnimationParameter()
    {
        animator.SetBool("IsHit", true);
    }
    
    void TryDisableHitAnimationParameter(int countSlow)
    {
        if(countSlow == 0)
        animator.SetBool("IsHit", false);
    }
    void EnableIsBeginThrow()
    {
        animator.SetBool("isBeginThrow", true);
    }

    void DisableIsThrow()
    {
        animator.SetBool("isBeginThrow", false);
    }
    void SetBlendTreeMovement(float speed) => animator.SetFloat("speed", speed/_playerMovement.stats.Speed);

    void SetIsShoot(bool value) => animator.SetBool("IsShoot", value);
    


    void DisableIsShoot() => SetIsShoot(false);




}
