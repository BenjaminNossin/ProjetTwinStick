using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemTrajectoryPreview : MonoBehaviour
{
    [SerializeField] private ItemThrowData throwData;
     [SerializeField] private GameObject previewPrefab;
     [SerializeField] private GameObject targetPrefab;
    private Item _item;
    private GameObject _previewInstance;
    private GameObject _targetInstance;
    private Vector3 _startPosition;
    private Vector3 _direction;
    private float _chargeTime;
    
    private void OnEnable()
    {
        _previewInstance = Instantiate(previewPrefab);
        _previewInstance.SetActive(false);
        _targetInstance = Instantiate(targetPrefab);
        _targetInstance.SetActive(false);
        _item = GetComponent<Item>();
        _item.OnItemStateChange += OnItemStateChange;
    }
    
    private void OnDisable()
    {
        Destroy(_previewInstance);
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
        _previewInstance.SetActive(false);
        _targetInstance.SetActive(false);
    }

    public void ActivateTargetInstance()
    {
        _targetInstance.SetActive(true); 
        _previewInstance.SetActive(false);
        _targetInstance.transform.position = _previewInstance.transform.position;
    }
    
    public void UpdatePreview(Vector3 startPosition, Vector3 Direction, float chargeTime)
    {
        _startPosition = startPosition;
        _direction = Direction;
        _chargeTime = chargeTime;
        
        _previewInstance.SetActive(true);
        Vector3 targetPos = _startPosition + _direction * throwData.GetThrowDistance(_chargeTime);
        _previewInstance.transform.position = targetPos;
    }

}
