using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmSpawnerManager : MonoBehaviour
{
    [SerializeField] private EnemyPool _enemyPool;
    [SerializeField] private List<SwarmSpawnersArea> allSwarmSpawnersByArea = new List<SwarmSpawnersArea>();

    private void Start()
    {
        GameEventTimelineReader.AddGameEventSetter(typeof(SwarmEvent), SetSwarmEvent);
    }

    void SetSwarmEvent(GameEvent gameEvent)
    {
        
    }
}
