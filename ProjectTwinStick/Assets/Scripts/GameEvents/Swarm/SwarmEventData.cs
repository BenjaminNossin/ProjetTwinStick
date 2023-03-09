using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SwarmEventData", menuName = "GameEvents/SwarmEvent", order = 2)]
public class SwarmEventData : GameEventAreaData
{
    public SwarmWave[] SwarmWaves;
    public override Type GetTypeEvent()
    {
        return typeof(SwarmEvent);
    }

    private void OnValidate()
    {
        AreaTargetCount = 1;
    }
}
