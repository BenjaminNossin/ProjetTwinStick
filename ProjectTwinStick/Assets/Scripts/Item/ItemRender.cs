using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemRender : MonoBehaviour
{
    private Item _item;

    [SerializeField] private GameObject dropStateFx;
    [SerializeField] private Animator _animator;
    [SerializeField] private string itemDropAnimationState;
    [SerializeField] private string itemThrowAnimationState;
    [SerializeField] private string itemUseAnimationParameter;
    [SerializeField] private string itemHeldAnimationState;
    [SerializeField] private MeshRenderer[] allUpgradedItemRenderers;

    private void OnEnable()
    {
        _item = GetComponent<Item>();
        _item.OnItemStateChange += OnItemStateChange;
        _item.OnShoot += EnableShootAnimationParameter;
        _item.OnUnShoot += DisableShootAnimationParameter;
        _item.OnItemUpgradeChanged += SetUpgradeShaderParameter;
    }

    private void SetUpgradeShaderParameter(int upgradeCount)
    {
    
        for (int i = 0; i < allUpgradedItemRenderers.Length; i++)
        {
          
            for (int j = 0; j < allUpgradedItemRenderers[i].materials.Length; j++)
            {
        
                if (allUpgradedItemRenderers[i].materials[j].HasFloat("_Level"))
                {
                    allUpgradedItemRenderers[i].materials[j].SetFloat("_Level", upgradeCount);
                  
                    break;
                }
             
            }
        }
    }

    private void EnableShootAnimationParameter()
    {
        if (_animator)
            _animator.SetBool(itemUseAnimationParameter, true);
    }

    private void DisableShootAnimationParameter()
    {
        if (_animator)
            _animator.SetBool(itemUseAnimationParameter, false);
    }


    private void OnDisable()
    {
        _item.OnItemStateChange -= OnItemStateChange;
    }

    private void OnItemStateChange(ItemState state)
    {
        switch (state)
        {
            case ItemState.Bouncing:
            {
                dropStateFx.SetActive(false);
                break;
            }
            case ItemState.Dropped:
            {
                _animator.Play(itemDropAnimationState);
                dropStateFx.SetActive(true);
                break;
            }
            case ItemState.Thrown:
            {
           
                    _animator.Play(itemThrowAnimationState);
                dropStateFx.SetActive(false);
                break;
            }
            case ItemState.Held:
            {
                _animator.Play(itemHeldAnimationState);
                dropStateFx.SetActive(false);
                break;
            }
        }
    }
}