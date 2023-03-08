using UnityEngine;

public class LobbyState : State
{
    public override void OnStateEnter()
    {
        // context.TransitionTo(new GameState());
        Debug.Log("Initializing Lobby"); 
        context.PlayerInputManager.onPlayerJoined += BindOnPlayerJoined;
        context.PlayerInputManager.onPlayerLeft += BindOnPlayerLeave;

        GameManager.Instance.OnLobbyStart(); 
    }

    public override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }
}
