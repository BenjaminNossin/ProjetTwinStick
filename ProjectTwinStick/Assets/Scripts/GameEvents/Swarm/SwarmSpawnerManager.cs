using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SwarmSpawnerManager : MonoBehaviour
{
    [SerializeField] private EnemyPoolManager enemyPoolManager;
    [SerializeField] private SwarmSpawnersArea[] _allSwarmSpawnersByArea = new SwarmSpawnersArea[] { };
    [SerializeField] private SwarmSpawnersArea[] _swarmSpawnersAvailableByArea = new SwarmSpawnersArea[4];


    private void Start()
    {
        GameEventTimelineReader.AddGameEventSetter(typeof(SwarmEvent), SetSwarmEvent);
    }

    private void OnValidate()
    {
        _swarmSpawnersAvailableByArea = new SwarmSpawnersArea[4];
        for (int i = 0; i < _allSwarmSpawnersByArea.Length; i++)
        {
            _swarmSpawnersAvailableByArea[i] = new SwarmSpawnersArea();
            _swarmSpawnersAvailableByArea[i].Area = _allSwarmSpawnersByArea[i].Area;
            _swarmSpawnersAvailableByArea[i].SwarmSpawners =
                new List<SwarmSpawner>(_allSwarmSpawnersByArea[i].SwarmSpawners);
        }
    }

    public void ResetSpawnerArea(Area area)
    {
        for (int i = 0; i < _swarmSpawnersAvailableByArea.Length; i++)
        {
            if (_swarmSpawnersAvailableByArea[i].Area == area)
            {
                _swarmSpawnersAvailableByArea[i].SwarmSpawners =
                    new List<SwarmSpawner>(_allSwarmSpawnersByArea[i].SwarmSpawners);
                break;
            }

        }
    }

    public void GenerateEnemies(Area area, int enemyCount)
    {
        return; 

        for (int i = 0; i < _swarmSpawnersAvailableByArea.Length; i++)
        {
            if (_swarmSpawnersAvailableByArea[i].Area == area)
            {
                for (int j = 0; j < enemyCount; j++)
                {
                    if (_swarmSpawnersAvailableByArea[i].SwarmSpawners.Count != 0)
                    {
                        int randIndex = Random.Range(0, _swarmSpawnersAvailableByArea[i].SwarmSpawners.Count);
                        var enemy = enemyPoolManager.enemyPools[0].GetFromPool();
                        enemy._pool = enemyPoolManager.enemyPools[0];
                        enemy.transform.position =
                            _swarmSpawnersAvailableByArea[i].SwarmSpawners[randIndex].transform.position;
                        enemy.Init(Vector3.zero); // PLACEHOLDER
                        _swarmSpawnersAvailableByArea[i].SwarmSpawners.RemoveAt(randIndex);
                    }
                }

                break;
            }
        }
    }

    void SetSwarmEvent(GameEvent gameEvent)
    {
        var swarmEvent = (SwarmEvent)gameEvent;
        swarmEvent.swarmSpawnerManager = this;
    }
}