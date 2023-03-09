using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameEventTargetAreaPlacer : MonoBehaviour
{
    [SerializeField] private GameEventTimelineReader _timelineReader;
    private Dictionary<Area, List<GameEvent>> _currentEventInAreas = new();
    private Dictionary<Area, GameEvent> _lastEventInAreas = new();

    private void Start()
    {
        _timelineReader.gameEventCreatedCallback += PlaceEvent;
        for (int i = 0; i < 3; i++)
        {
            _currentEventInAreas.Add((Area)i, new List<GameEvent>());
            _lastEventInAreas.Add((Area)i, null);
        }
    }

    private void PlaceEvent(GameEvent gameEvent)
    {
        if (!(gameEvent is GameEventArea gameEventArea)) return;
        var so = (GameEventAreaData)gameEventArea.GetSO();
        List<Area> areas = new List<Area>();
        for (int i = 0; i < so.AreaTargetCount; i++)
        {
            foreach (var currentEventInArea in _currentEventInAreas)
            {
                bool isValidedCurrentTagBlockerCondition = true;
                for (int j = 0; j < currentEventInArea.Value.Count; j++)
                {
                    var currentEventCheckedSO = (GameEventAreaData)currentEventInArea.Value[j].GetSO();
                    if (so.CurrentEventInAreaBlockerTag.Contains(currentEventCheckedSO.TagEvent))
                    {
                        isValidedCurrentTagBlockerCondition = false;
                        break;
                    }
                }

                if (isValidedCurrentTagBlockerCondition)
                {
                    var lastEventCheckedSO = (GameEventAreaData)_lastEventInAreas[currentEventInArea.Key].GetSO();
                    if (so.LastEventInAreaBlockerTag.Contains(lastEventCheckedSO.TagEvent))
                    {
                        currentEventInArea.Value.Add(gameEvent);
                        areas.Add(currentEventInArea.Key);
                        // place
                    }
                }
            }
        }

        if (areas.Count == so.AreaTargetCount)
        {
            gameEventArea.SetAreas(areas.ToArray());
            gameEventArea.EndConditionCallback += RemoveEventArea;
        }
        else
        {
            throw new Exception("Don't find area available for launch event");
        }
    }

    public void RemoveEventArea(GameEvent gameEvent)
    {
        var gameEventArea = (GameEventArea)gameEvent;
        var gameEventAreaSO = (GameEventAreaData)gameEvent.GetSO();
        var areas = gameEventArea.GetArea();
        for (int i = 0; i < areas.Length; i++)
        {
            _currentEventInAreas[areas[i]].Remove(gameEvent);
            if (gameEventAreaSO.IsRememberEvent)
                _lastEventInAreas[areas[i]] = gameEvent;
        }
    }
}