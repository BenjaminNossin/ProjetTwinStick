using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventCallbackListener : MonoBehaviour
{
    [SerializeField] UnityEvent<GameEvent> response;
    [SerializeField] private GameEventCallback _gameEventCallback;

    void OnEnable()
    {
        _gameEventCallback.RegisterListener(this);
    }

    void OnDisable()
    {
        _gameEventCallback.UnregisterListener(this);
    }

    public void OnEventRaised(GameEvent gameEvent) => response?.Invoke(gameEvent);
}