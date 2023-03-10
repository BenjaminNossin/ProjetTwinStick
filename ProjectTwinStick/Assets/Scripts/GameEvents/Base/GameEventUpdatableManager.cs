using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventUpdatableManager : MonoBehaviour
{
    private List<IGameEventUpdatable> currentGameEventUpdatables = new List<IGameEventUpdatable>();

    [SerializeField] private GameEventTimelineReader _timelineReader;

    private bool isActive;
    private void Start()
    {
        _timelineReader.gameEventCreatedCallback += AddGameEventUpdatable;
        _timelineReader.onGameStartCallback += OnGameStart;
        _timelineReader.onGameOverCallback += OnGameOver;
    }

    void OnGameStart()
    {
        currentGameEventUpdatables.Clear();
        isActive = true;
    }

    void OnGameOver()
    {
        isActive = false; 
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
        if (!isActive) return;
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