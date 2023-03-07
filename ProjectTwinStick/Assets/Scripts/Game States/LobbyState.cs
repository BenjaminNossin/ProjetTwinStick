using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public override void Request()
    {
        context.TransitionTo(new GameState());
    }

    private void BindOnPlayerJoined()
    {

    }
}
