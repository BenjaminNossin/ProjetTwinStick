using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Game.Systems.GlobalFramework;

// TODO: extract the BarricadeAreaData-related stuff in the SwarmSpawner system

[Serializable]
public class BarricadeAreaData
{
    public Area Area;
    public Transform[] barricadeTransforms = new Transform[3];  
}

public class MeteorSpawnerManager : MonoBehaviour
{
    [SerializeField] private EnemyPoolManager enemyPoolManager;
    [SerializeField] private MeteorSpawnersArea[] _allMeteorSpawnersByArea = new MeteorSpawnersArea[] { };
    [SerializeField] private MeteorSpawnersArea[] _meteorSpawnersAvailableByArea = new MeteorSpawnersArea[4];

    [SerializeField, Range(1, 4)] private int barricadesPerArea = 3; 
    [SerializeField] private BarricadeAreaData[] barricadeArea = new BarricadeAreaData[4]; 
    private Dictionary<Area, Transform[]> barricadeAreasData = new();

    private void Start()
    {
        GameEventTimelineReader.AddGameEventSetter(typeof(MeteorEvent), SetMeteorEvent);

        Init(); 
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

    private void Init()
    {
        for (int i = 0; i < barricadeArea.Length; i++)
        {
            if (!barricadeAreasData.ContainsKey(barricadeArea[i].Area))
            {
                barricadeAreasData.Add(barricadeArea[i].Area, barricadeArea[i].barricadeTransforms);
            }
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

    public void GenerateMeteor(Area area, int enemyCount)
    {
        for (int i = 0; i < _meteorSpawnersAvailableByArea.Length; i++)
        {
            if (_meteorSpawnersAvailableByArea[i].Area == area)
            {
                for (int j = 0; j < enemyCount; j++)
                {
                    if (_meteorSpawnersAvailableByArea[i].MeteorSpawners.Count != 0)
                    {
                        int meteorRandIndex = Random.Range(0, _meteorSpawnersAvailableByArea[i].MeteorSpawners.Count);
                        int targetRandIndex = Random.Range(0, barricadesPerArea+1); 

                        var meteor = enemyPoolManager.meteorPools[0].GetFromPool();
                        meteor._pool = enemyPoolManager.meteorPools[0];
                        meteor.transform.position =
                            _meteorSpawnersAvailableByArea[i].MeteorSpawners[meteorRandIndex].transform.position;

                        Transform[] allowedTargetsTransf = barricadeAreasData[area];
                        //meteor.Init(allowedTargetsTransf[targetRandIndex].position); 
                        meteor.Init(GameManager.Instance.ShipCoreObj.transform.position); 
                        _meteorSpawnersAvailableByArea[i].MeteorSpawners.RemoveAt(meteorRandIndex);
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
