using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationEvent : GameEvent, IGameEventUpdatable
{
    private RotationEventData rotationEventData;

    public RotationEventManager rotationEventManager;
    float timer;

    public override void Raise()
    {
        base.Raise();
        timer = rotationEventData.rotationTime;
    }

    public override void SetSO(GameEventData data)
    {
        rotationEventData = (RotationEventData)data;
    }

    public override GameEventData GetSO()
    {
        return rotationEventData;
    }

    public void OnUpdate()
    {
        if (timer >= 0)
        {
            rotationEventManager.RotateCore(((rotationEventData.rotationValue * 90) / rotationEventData.rotationTime) * Time.deltaTime, rotationEventData.clockwise);
            
        }
        else
        {
            EndConditionCallback?.Invoke(this);
        }

        timer -= Time.deltaTime;
    }

}