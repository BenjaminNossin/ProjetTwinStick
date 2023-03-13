using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RotationEventData", menuName = "GameEvents/RotationEvent", order = 3)]
public class RotationEventData : GameEventData
{
    public float rotationTime;
    [Range(1, 4)] public int rotationValue;
    public bool clockwise;
    public override Type GetTypeEvent() 
    {
        return typeof(RotationEvent);
    }


}
