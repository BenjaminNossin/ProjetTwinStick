using Game.Systems.GlobalFramework.States;
using UnityEngine;


public class QuitState : State
{
    public override void OnStateEnter()
    {
        Debug.Log("initializing quit state");
    }

    public override void OnStateExit()
    {
        //
    }
}
