using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyState : State
{
    public override void OnStateEnter()
    {
        // context.TransitionTo(new GameState());
        Debug.Log("Initializing Lobby"); 
        context.PlayerInputManager.onPlayerJoined += BindOnPlayerJoined;
        context.PlayerInputManager.onPlayerLeft += BindOnPlayerLeave; 
    }

    public override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }
}
