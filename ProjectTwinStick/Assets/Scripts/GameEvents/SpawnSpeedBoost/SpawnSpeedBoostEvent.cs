using System.Collections;
using System.Collections.Generic;
using Game.Systems.AI;
using UnityEngine;

public class SpawnSpeedBoostEvent : GameEvent
{
    private SpawnSpeedBoostEventData _eventData;
    public override void Raise()
    {
        base.Raise();
        WaveManager waveManager = MonoBehaviour.FindObjectOfType<WaveManager>();
        waveManager.ChangeSpawnerParams(_eventData.newParams);
    }
    public override void SetSO(GameEventData data)
    {
        _eventData = data as SpawnSpeedBoostEventData;
    }

    public override GameEventData GetSO()
    {
        return _eventData;
    }
}
