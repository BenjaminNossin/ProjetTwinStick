using Game.Systems.GlobalFramework;
using Game.Systems.GlobalFramework.States;
using UnityEngine;

public class WinState : State
{
    public override void OnStateEnter()
    {
        Debug.Log("Initializing Game Won State");
        DeactivateAllPlayerControllers();
        ClearPlayerListFromContext();
    }

    public override void OnStateExit()
    {

    }
}
