using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventTimelineReader : MonoBehaviour
{
    [SerializeField] private GameEventTimelineSO _gameEventTimeline;
    private int timeCodeIndex = 0;
    private float timer;

    private static Dictionary<Type, List<Action<GameEvent>>> allGameEventSetters =
        new Dictionary<Type, List<Action<GameEvent>>>();
    public event Action<GameEvent> gameEventCreatedCallback; 
    private void Update()
    {
        if (_gameEventTimeline.GetTimeCode(timeCodeIndex) < timer)
        {
            var gameEventsToCreated = _gameEventTimeline.GetGameEvents(timeCodeIndex);
            for (int i = 0; i < gameEventsToCreated.Length; i++)
            {
                GameEvent newEvent =(GameEvent) Activator.CreateInstance(gameEventsToCreated[i].GetTypeEvent());
                SetNewEvent(newEvent, gameEventsToCreated[i]);
                gameEventCreatedCallback?.Invoke(newEvent);
                newEvent.Raise();
                timeCodeIndex++;
            }
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    private static void SetNewEvent(GameEvent newEvent, GameEventData gameEventsToCreated)
    {
        newEvent.SetSO(gameEventsToCreated);
        foreach (var setter in allGameEventSetters[gameEventsToCreated.GetTypeEvent()])
        {
            setter.Invoke(newEvent);
        }
    }

    public static void AddGameEventSetter(Type eventType, Action<GameEvent> setter)
    {
        if (!allGameEventSetters.ContainsKey(eventType))
        {
            allGameEventSetters.Add(eventType, new List<Action<GameEvent>>());
        }
        allGameEventSetters[eventType].Add(setter);
    }

    public static void RemoveGameEventSetter(Type eventType, Action<GameEvent> setter)
    {
        if (!allGameEventSetters.ContainsKey(eventType))
        {
            return;
        }
        allGameEventSetters[eventType].Remove(setter);
        if (allGameEventSetters[eventType].Count == 0)
            allGameEventSetters.Remove(eventType);
    }
}