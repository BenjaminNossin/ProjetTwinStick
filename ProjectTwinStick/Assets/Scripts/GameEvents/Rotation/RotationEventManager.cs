using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationEventManager : MonoBehaviour
{
    [SerializeField] private GameObject core;
    void Start()
    {
        GameEventTimelineReader.AddGameEventSetter(typeof(RotationEvent), SetRotationEvent);
    }
    public void RotateCore(float rotationValue, bool clockwise)
    {
        if (!clockwise)
        {
            rotationValue = -rotationValue;
        }

        core.transform.Rotate(new Vector3(0, rotationValue, 0));
    }

    void SetRotationEvent(GameEvent gameEvent)
    {
        var rotationEvent = (RotationEvent)gameEvent;
        rotationEvent.rotationEventManager = this;
    }
}
