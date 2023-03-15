using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialPopupEvent", menuName = "GameEvents/TutorialPopupEvent", order = 0)]
public class TutorialPopupEventData : GameEventData
{
    [TextArea(3, 10)]
    public string Text;
    public float Duration = 10f;

    public override Type GetTypeEvent()
    {
        return typeof (TutorialPopupEvent);
    }
}
