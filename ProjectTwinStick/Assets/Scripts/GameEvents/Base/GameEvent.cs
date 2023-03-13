using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class GameEvent
{
    public Action<GameEvent> EndConditionCallback;

    public virtual void Raise()
    {
        if(GetSO().callbacks.Length == 0) return;
        foreach (var callback in GetSO().callbacks)
        {
            callback.Raise(this);
        }
    }
    public abstract void SetSO(GameEventData data);
    
    public abstract GameEventData GetSO();
    
     
}
