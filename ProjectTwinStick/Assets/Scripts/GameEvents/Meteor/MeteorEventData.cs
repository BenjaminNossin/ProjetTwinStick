using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeteorEventData", menuName = "GameEvents/MeteorEvent", order = 3)]
public class MeteorEventData : GameEventAreaData
{
    public MeteorWave[] MeteorWaves;

    public override Type GetTypeEvent()
    {
        return typeof(MeteorEvent);
    }
}
