using Game.Systems.GlobalFramework;
using Game.Systems.GlobalFramework.States;
using UnityEngine;

public class CreditsState : State
{
    public override void OnStateEnter()
    {
        GameManager.Instance.OnShowCredits(); 
    }

    public override void OnStateExit()
    {
        //
    }
}
