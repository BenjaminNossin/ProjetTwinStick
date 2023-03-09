using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmEvent : GameEventArea
{
    private SwarmEventData swarmEventData;

    public override void Raise()
    {
        throw new System.NotImplementedException();
    }

    public override void SetSO(GameEventData data)
    {
        swarmEventData = (SwarmEventData)data;
    }

    public override GameEventData GetSO()
    {
        return swarmEventData;
    }
}