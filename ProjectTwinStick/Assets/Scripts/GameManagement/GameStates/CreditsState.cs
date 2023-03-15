using Game.Systems.GlobalFramework.States;
using UnityEngine;

public class CreditsState : State
{
    public override void OnStateEnter()
    {
        Debug.Log("initializing Credits state");
    }

    public override void OnStateExit()
    {
        //
    }
}
