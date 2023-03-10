using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HelperPSR.ListArray;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameEventTargetAreaPlacer : MonoBehaviour
{
    [SerializeField] private GameEventTimelineReader _timelineReader;
    private Dictionary<Area, List<GameEvent>> _currentEventInAreas = new();
    private Dictionary<Area, GameEvent> _lastEventInAreas = new();

    [SerializeField] private Area[] _suffledAreas;

    private bool isActive;
    private void Start()
    {
        _timelineReader.gameEventCreatedCallback += PlaceEvent;
        _timelineReader.onGameStartCallback += OnGameStart;
        _timelineReader.onGameOverCallback += OnGameOver;
    }

    void OnGameStart()
    {
        _currentEventInAreas.Clear();
        _lastEventInAreas.Clear();
        for (int i = 0; i < 4; i++)
        {
            _currentEventInAreas.Add((Area)i, new List<GameEvent>());
            _lastEventInAreas.Add((Area)i, null);
        }
        isActive = true;
    }

    void OnGameOver()
    {
        isActive = false; 
    }
    

    private void PlaceEvent(GameEvent gameEvent)
    {
        if (!(gameEvent is GameEventArea gameEventArea)) return;
        var so = (GameEventAreaData)gameEventArea.GetSO();
        List<Area> resultAreas = new List<Area>();
        System.Random random = new System.Random();
        for (int i = 0; i < so.AreaTargetCount; i++)
        {  
            random.ShuffleArray(_suffledAreas);
            for (int k = 0; k < _suffledAreas.Length; k++)
            {
                    
            bool isValidedCurrentTagBlockerCondition = true;
            for (int j = 0; j < _currentEventInAreas[_suffledAreas[k]].Count; j++)
            {
             
                var currentEventCheckedSO = (GameEventAreaData)_currentEventInAreas[_suffledAreas[k]][j].GetSO();
                if (so.CurrentEventInAreaBlockerTag.Contains(currentEventCheckedSO.TagEvent))
                {
                    isValidedCurrentTagBlockerCondition = false;
                }
            }

            if (isValidedCurrentTagBlockerCondition)
            {
                if (_lastEventInAreas[_suffledAreas[k]] == null)
                {
                    _currentEventInAreas[_suffledAreas[k]].Add(gameEvent);
                    resultAreas.Add(_suffledAreas[k]);
                    break;
                }
                else
                {
                    var lastEventCheckedSO = (GameEventAreaData)_lastEventInAreas[_suffledAreas[k]].GetSO();
                    if (!so.LastEventInAreaBlockerTag.Contains(lastEventCheckedSO.TagEvent))
                    {
                        _currentEventInAreas[_suffledAreas[k]].Add(gameEvent);
                        resultAreas.Add(_suffledAreas[k]);
                        break;
                    }
                }
            }
            }
        }

        if (resultAreas.Count == so.AreaTargetCount)
        {
            gameEventArea.SetAreas(resultAreas.ToArray());
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