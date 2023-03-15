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
        Debug.Log("QUIT STATE: unbinding input callbacks");
        context.PlayerInputManager.onPlayerJoined -= BindOnPlayerJoined;
        context.PlayerInputManager.onPlayerLeft -= BindOnPlayerLeave;
    }
}
