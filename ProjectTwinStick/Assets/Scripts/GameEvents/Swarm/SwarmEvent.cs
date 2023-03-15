using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmEvent : GameEventArea, IGameEventUpdatable
{
    private SwarmEventData swarmEventData;
    public SwarmSpawnerManager swarmSpawnerManager;
    public int currentSwarmWaveIndex = 0;
    public float _timer;

    public override void Raise()
    {
        base.Raise();
        swarmSpawnerManager.ResetSpawnerArea(targetAreas[0]);
    }

    public override void SetSO(GameEventData data)
    {
        swarmEventData = (SwarmEventData)data;
    }

    public override GameEventData GetSO()
    {
        return swarmEventData;
    }

    public void OnUpdate()
    {
        if (_timer > swarmEventData.SwarmWaves[currentSwarmWaveIndex].timeBeforeLaunchWave)
        {
            GenerateWave();
            _timer = 0;
            currentSwarmWaveIndex++;
            if (currentSwarmWaveIndex == swarmEventData.SwarmWaves.Length)
            {
                EndConditionCallback?.Invoke(this);
            }
        }

        _timer += Time.deltaTime;
    }

    private void GenerateWave()
    {
        Debug.Log(swarmEventData.name + "  area" + targetAreas[0]);
        swarmSpawnerManager.GenerateEnemies(targetAreas[0],
            swarmEventData.SwarmWaves[currentSwarmWaveIndex].enemyCount);
        swarmSpawnerManager.ResetSpawnerArea(targetAreas[0]);
    }
}