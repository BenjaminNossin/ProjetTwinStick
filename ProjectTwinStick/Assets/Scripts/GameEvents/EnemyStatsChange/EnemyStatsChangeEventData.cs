using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatsChangeEventData", menuName = "GameEvents/EnemyStatsChangeEvent", order = 4)]
public class EnemyStatsChangeEventData : GameEventData
{
    public EnemyStats NewStats;

    public override Type GetTypeEvent()
    {
        return typeof(EnemyStatsChangeEvent);
    }
}
