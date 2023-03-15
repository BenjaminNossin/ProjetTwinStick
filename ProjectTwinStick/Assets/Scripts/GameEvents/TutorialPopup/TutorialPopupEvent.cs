using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopupEvent : GameEvent
{
    private TutorialPopupEventData _data;

    public override void Raise()
    {
        base.Raise();
        Debug.Log("TutorialPopupEvent raised");
        UITutorialPopup popup = MonoBehaviour.FindObjectOfType<UITutorialPopup>();
        popup.SetPopup(_data.Text, _data.Duration);
    }
    
    public override void SetSO(GameEventData data)
    {
        _data = data as TutorialPopupEventData;
    }

    public override GameEventData GetSO()
    {
        return _data;
    }
}
