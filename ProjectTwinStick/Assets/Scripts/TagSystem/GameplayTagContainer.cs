using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameplayTagContainer : MonoBehaviour
{
    [SerializeField] private List<GameplayTag> StartingTags;
    private List<GameplayTag> tags = new List<GameplayTag>();

    private void Awake()
    {
        tags.AddRange(StartingTags);
    }

    public event Action OnTagListChanged;
    
    public void AddTag(GameplayTag tag)
    {
        tags.Add(tag);
        OnTagListChanged?.Invoke();
    }
    
    public void RemoveTag(GameplayTag tag)
    {
        tags.Remove(tag);
        OnTagListChanged?.Invoke();
    }

    public int GetTagCount(GameplayTag tag)
    {
        int count = 0;
        foreach (GameplayTag t in tags)
        {
            if (t == tag)
            {
                count++;
            }
        }
        return count;
    }
    
    public bool HasTag(GameplayTag tag)
    {
        return tags.Contains(tag);
    }
}
