using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GameEventCallback", menuName = "GameEvents/GameEventCallback", order = 2)]
public class GameEventCallback : ScriptableObject
{
  private readonly List<GameEventCallbackListener> _eventCallbackListeners = new List<GameEventCallbackListener>();
  public void Raise(GameEvent gameEvent)
  {
    for (int i = 0; i < _eventCallbackListeners.Count; i++)
    { 
       _eventCallbackListeners[i].OnEventRaised(gameEvent);
    }
  }

  public void RegisterListener(GameEventCallbackListener listener)
  {
    if(!_eventCallbackListeners.Contains(listener))
    _eventCallbackListeners.Add(listener);
  }

  public void UnregisterListener(GameEventCallbackListener listener)
  {
    _eventCallbackListeners.Remove(listener);
  }
}
