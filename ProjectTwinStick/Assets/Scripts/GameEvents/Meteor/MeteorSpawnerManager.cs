using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MeteorSpawnerManager : MonoBehaviour
{
    [SerializeField] private EnemyPoolManager enemyPoolManager;
    [SerializeField] private MeteorSpawnersArea[] _allMeteorSpawnersByArea = new MeteorSpawnersArea[] { };
    [SerializeField] private MeteorSpawnersArea[] _meteorSpawnersAvailableByArea = new MeteorSpawnersArea[4];

    private void Start()
    {
        GameEventTimelineReader.AddGameEventSetter(typeof(MeteorEvent), SetMeteorEvent);
    }

    private void OnValidate()
    {
        _meteorSpawnersAvailableByArea = new MeteorSpawnersArea[4];
        for (int i = 0; i < _allMeteorSpawnersByArea.Length; i++)
        {
            _meteorSpawnersAvailableByArea[i] = new MeteorSpawnersArea();
            _meteorSpawnersAvailableByArea[i].Area = _allMeteorSpawnersByArea[i].Area;
            _meteorSpawnersAvailableByArea[i].MeteorSpawners =
                new List<MeteorSpawner>(_allMeteorSpawnersByArea[i].MeteorSpawners);
        }
    }

    public void ResetSpawnerArea(Area area)
    {
        for (int i = 0; i < _meteorSpawnersAvailableByArea.Length; i++)
        {
            if (_meteorSpawnersAvailableByArea[i].Area == area)
            {
                _meteorSpawnersAvailableByArea[i].MeteorSpawners =
                    new List<MeteorSpawner>(_allMeteorSpawnersByArea[i].MeteorSpawners);
                break;
            }

        }

    }

    public void GenerateEnemies(Area area, int enemyCount)
    {
        return;

        for (int i = 0; i < _meteorSpawnersAvailableByArea.Length; i++)
        {
            if (_meteorSpawnersAvailableByArea[i].Area == area)
            {
                for (int j = 0; j < enemyCount; j++)
                {
                    if (_meteorSpawnersAvailableByArea[i].MeteorSpawners.Count != 0)
                    {
                        int randIndex = Random.Range(0, _meteorSpawnersAvailableByArea[i].MeteorSpawners.Count);
                        var enemy = enemyPoolManager.enemyPools[0].GetFromPool();
                        enemy._pool = enemyPoolManager.enemyPools[0];
                        enemy.transform.position =
                            _meteorSpawnersAvailableByArea[i].MeteorSpawners[randIndex].transform.position;
                        enemy.Init(Vector3.zero); // PLACEHOLDER
                        _meteorSpawnersAvailableByArea[i].MeteorSpawners.RemoveAt(randIndex);
                    }
                }

                break;
            }
        }
    }

    void SetMeteorEvent(GameEvent gameEvent)
    {
        var meteorEvent = (MeteorEvent)gameEvent;
        meteorEvent.meteorSpawnerManager = this;
    }
}
