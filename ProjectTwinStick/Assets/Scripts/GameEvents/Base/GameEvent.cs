using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class GameEvent
{
    public Action<GameEvent> EndConditionCallback;
    public abstract void Raise();
    public abstract void SetSO(GameEventData data);
    
    public abstract GameEventData GetSO();
}
