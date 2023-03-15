using Game.Systems.GlobalFramework;
using Game.Systems.GlobalFramework.States;
using UnityEngine;


public class QuitState : State
{
    public override void OnStateEnter()
    {
        GameManager.Instance.OnGameQuit(); 
    }

    public override void OnStateExit()
    {
        //
    }
}
