using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorEvent : GameEventArea, IGameEventUpdatable
{
    private MeteorEventData meteorEventData;
    public MeteorSpawnerManager meteorSpawnerManager;

    public override GameEventData GetSO()
    {
        throw new System.NotImplementedException();
    }

    public override void Raise()
    {
        throw new System.NotImplementedException();
    }

    public override void SetSO(GameEventData data)
    {
        throw new System.NotImplementedException();
    }

    public void OnUpdate()
    {
        throw new System.NotImplementedException();
    }
}
