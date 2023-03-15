
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public struct SlowInstance
{
    public SlowSO slowSO;
    public float timeRemaining;
}

public class SlowManager : MonoBehaviour
{
    public event Action<float> OnSlowMultiplierChanged;
    public event Action OnSlowAdded;
    public event Action<int> OnSlowRemove;
    private List<SlowInstance> _instances = new List<SlowInstance>();
    private float _slowMultiplier = 1f;

    [SerializeField] GameplayTagContainer _tagContainer;
    
    //Adds a slow to the list of slows
    public void AddSlow(SlowSO slowSo)
    {
        Debug.Log(slowSo.TagsToAdd.Count);
        var instance = new SlowInstance();
        instance.slowSO = slowSo;
        instance.timeRemaining = slowSo.SlowDuration;
        for (int i = 0; i < slowSo.TagsToAdd.Count; i++)
        {
            _tagContainer.AddTag(slowSo.TagsToAdd[i]);
        }
        _instances.Add(instance);
        OnSlowAdded?.Invoke();
    }

    //removes one instance matching the provided SO
    public void RemoveSlow(SlowSO slowSo)
    {
        for(int i = _instances.Count - 1; i >= 0; i--)
        {
            if(_instances[i].slowSO == slowSo)
            {
                for (int tagIndex = 0; tagIndex < slowSo.TagsToAdd.Count; tagIndex++)
                {
                    _tagContainer.RemoveTag(slowSo.TagsToAdd[tagIndex]);
                }
                _instances.RemoveAt(i);
                OnSlowRemove?.Invoke(_instances.Count);
                UpdateSlowMultiplier();
                return;
            }
        }
        Debug.Log("Couldn't find any matching slow instance to remove");
    }
    
    //Reduces time on all slows and removes them if they are done (unless they are indefinite). Updates the slow multiplier.
    public void Update()
    {
        for (int i = _instances.Count - 1; i >= 0; i--)
        {
            var instance = _instances[i];
            if (instance.slowSO.IndefiniteSlow)
            {
                continue;
            }
            instance.timeRemaining -= Time.deltaTime;
            if (instance.timeRemaining <= 0)
            {
                _instances.RemoveAt(i);
                for (int tagIndex = 0; tagIndex < instance.slowSO.TagsToAdd.Count; tagIndex++)
                {
                    _tagContainer.RemoveTag(instance.slowSO.TagsToAdd[tagIndex]);
                }
                OnSlowRemove?.Invoke(_instances.Count);
            }
            else
            {
                _instances[i] = instance;
            }
        }
        
        UpdateSlowMultiplier();
    }
    
    //Makes the lowest slow multiplier the current slow multiplier
    public void UpdateSlowMultiplier()
    {
        float oldSlowMultiplier = _slowMultiplier;
        _slowMultiplier = 1f;
        for (int i = 0; i < _instances.Count; i++)
        {
            var instance = _instances[i];
            _slowMultiplier = Mathf.Min(_slowMultiplier, instance.slowSO.SlowMultiplier);
        }
        if (Math.Abs(oldSlowMultiplier - _slowMultiplier) > 0.0001f)
        {
            OnSlowMultiplierChanged?.Invoke(_slowMultiplier);
        }
    }
    
    public float GetCurrentSlowMultiplier()
    {
        return _slowMultiplier;
    }
}
