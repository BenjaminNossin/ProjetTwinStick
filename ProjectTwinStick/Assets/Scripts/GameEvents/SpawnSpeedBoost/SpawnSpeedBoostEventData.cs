using System;
using System.Collections;
using System.Collections.Generic;
using Game.Systems.AI;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnSpeedBoostEventData", menuName = "GameEvents/SpawnSpeedBoostEventData", order = 0)]
public class SpawnSpeedBoostEventData : GameEventData
{
    public SpawnerParams newParams;
    public override Type GetTypeEvent()
    {
        return typeof(SpawnSpeedBoostEvent);
    }
}
