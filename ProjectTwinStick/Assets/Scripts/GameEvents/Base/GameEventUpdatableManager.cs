using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventUpdatableManager : MonoBehaviour
{
    private List<IGameEventUpdatable> currentGameEventUpdatables = new List<IGameEventUpdatable>();

    [SerializeField] private GameEventTimelineReader _timelineReader;

    private void Start()
    {
        _timelineReader.gameEventCreatedCallback += AddGameEventUpdatable;
    }
    
    public void AddGameEventUpdatable(GameEvent gameEvent)
    {
        if (gameEvent is IGameEventUpdatable gameEventUpdatable)
        {
            gameEvent.EndConditionCallback += RemoveGameEventUpdatable;
            currentGameEventUpdatables.Add(gameEventUpdatable);
        }
    }

    private void Update()
    {
        for (int i = 0; i < currentGameEventUpdatables.Count; i++)
        {
            currentGameEventUpdatables[i].OnUpdate();
        }
    }

    public void RemoveGameEventUpdatable(GameEvent gameEvent)
    {
        currentGameEventUpdatables.Remove((IGameEventUpdatable)gameEvent);
    }
}