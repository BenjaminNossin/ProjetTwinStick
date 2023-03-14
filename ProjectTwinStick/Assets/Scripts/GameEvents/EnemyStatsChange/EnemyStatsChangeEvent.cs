using System.Collections;
using System.Collections.Generic;
using Game.Systems.AI;
using UnityEngine;

public class EnemyStatsChangeEvent : GameEvent
{
    private EnemyStatsChangeEventData enemyStatsChangeEventData;
    
    public override void Raise()
    {
        base.Raise();
        Debug.Log("EnemyStatsChangeEvent raised");
        SwarmSpawnerManager swarmSpawnerManager = MonoBehaviour.FindObjectOfType<SwarmSpawnerManager>();
        swarmSpawnerManager.ChangeEnemyStats(enemyStatsChangeEventData.NewStats);
        WaveManager waveManager = MonoBehaviour.FindObjectOfType<WaveManager>();
        waveManager.ChangeEnemyStats(enemyStatsChangeEventData.NewStats);
        
    }
    
    public override void SetSO(GameEventData data)
    {
        enemyStatsChangeEventData = (EnemyStatsChangeEventData)data;
    }

    public override GameEventData GetSO()
    {
        return enemyStatsChangeEventData;
    }
}
