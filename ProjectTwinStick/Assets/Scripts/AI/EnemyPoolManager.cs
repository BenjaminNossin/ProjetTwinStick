using System;
using System.Collections;
using System.Collections.Generic;
using HelperPSR.Pool;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    [SerializeField] private PoolData<BasicAI>[] enemyPoolsData;
    [SerializeField] private PoolData<MeteorProjectileBehaviour>[] meteorPoolsData;

    public Pool<BasicAI>[] enemyPools;
    public Pool<MeteorProjectileBehaviour>[] meteorPools;


    private void Start()
    {
        enemyPools = new Pool<BasicAI>[enemyPoolsData.Length];
        meteorPools = new Pool<MeteorProjectileBehaviour>[meteorPoolsData.Length];

        for (int i = 0; i < enemyPools.Length; i++)
        {
            enemyPools[i] = new Pool<BasicAI>(enemyPoolsData[i].prefab, enemyPoolsData[i].startCount);
        }

        for (int i = 0; i < meteorPools.Length; i++)
        {
            meteorPools[i] = new Pool<MeteorProjectileBehaviour>(meteorPoolsData[i].prefab, meteorPoolsData[i].startCount);
        }
    }
}