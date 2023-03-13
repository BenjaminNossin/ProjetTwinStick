using System;
using System.Collections;
using System.Collections.Generic;
using HelperPSR.Pool;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    [SerializeField] private PoolData<BasicAI>[] enemyPoolsData;
    public Pool<BasicAI>[] enemyPools;
    
    

    private void Start()
    {
        enemyPools = new Pool<BasicAI>[enemyPoolsData.Length];
        for (int i = 0; i < enemyPools.Length; i++)
        {
            enemyPools[i] = new Pool<BasicAI>(enemyPoolsData[i].prefab, enemyPoolsData[i].startCount);
        }
    }



}