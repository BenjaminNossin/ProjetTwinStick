using UnityEngine;

public class MeteorEvent : GameEventArea, IGameEventUpdatable
{
    private MeteorEventData meteorEventData;
    public MeteorSpawnerManager meteorSpawnerManager;

    public int currentMeteorWaveIndex = 0;
    public float _timer;

    public override void Raise()
    {
        meteorSpawnerManager.ResetSpawnerArea(targetAreas[0]);
    }

    public override void SetSO(GameEventData data)
    {
        meteorEventData = (MeteorEventData)data;
    }

    public override GameEventData GetSO()
    {
        return meteorEventData;
    }

    public void OnUpdate()
    {
        if (_timer > meteorEventData.MeteorWaves[currentMeteorWaveIndex].timeBeforeLaunchWave)
        {
            GenerateWave();
            _timer = 0;
            currentMeteorWaveIndex++;
            if (currentMeteorWaveIndex == meteorEventData.MeteorWaves.Length)
            {
                EndConditionCallback?.Invoke(this);
            }
        }

        _timer += Time.deltaTime;
    }

    private void GenerateWave()
    {
        meteorSpawnerManager.GenerateMeteor(targetAreas[0], meteorEventData.MeteorWaves[currentMeteorWaveIndex].meteorCount);
        meteorSpawnerManager.ResetSpawnerArea(targetAreas[0]);
    }
}
