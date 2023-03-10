using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemTrajectoryPreview : MonoBehaviour
{
    [SerializeField] private ItemThrowData throwData;
    [SerializeField] private GameObject targetPrefab;

    private Item _item;
    private GameObject _targetInstance;


    private Vector3 _startPosition;
    private Vector3 _direction;
    private float _chargeTime;
    
    private void OnEnable()
    {
        _targetInstance = Instantiate(targetPrefab);
        _targetInstance.SetActive(false);
        _item = GetComponent<Item>();
        _item.OnItemStateChange += OnItemStateChange;
    }
    
    private void OnDisable()
    {
        Destroy(_targetInstance);
        _item.OnItemStateChange -= OnItemStateChange;
    }

    private void OnItemStateChange(ItemState state)
    {
        if (state != ItemState.Thrown)
        {
            CancelPreview();
        }
    }

    public void CancelPreview()
    {
        _targetInstance.SetActive(false);
    }
    
    public void UpdatePreview(Vector3 startPosition, Vector3 Direction, float chargeTime)
    {
        _startPosition = startPosition;
        _direction = Direction;
        _chargeTime = chargeTime;
        
        _targetInstance.SetActive(true);
        Vector3 targetPos = _startPosition + _direction * throwData.GetThrowDistance(_chargeTime);
        _targetInstance.transform.position = targetPos;
    }

}
