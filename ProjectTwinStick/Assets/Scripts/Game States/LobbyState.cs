using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyState : State
{
    public override void OnStateEnter()
    {
        throw new System.NotImplementedException();
        // setup
    }

    public override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }

    public override void Initialize()
    {
        // context.TransitionTo(new GameState());
        Debug.Log("Initializing Lobby"); 
        context.PlayerInputManager.onPlayerJoined += BindOnPlayerJoined;
        context.PlayerInputManager.onPlayerLeft += BindOnPlayerLeave; 
    }
}
