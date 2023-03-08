using UnityEngine;

public class LobbyState : State
{
    public override void OnStateEnter()
    {
        Debug.Log("Initializing Lobby"); 
        context.PlayerInputManager.onPlayerJoined += BindOnPlayerJoined;
        context.PlayerInputManager.onPlayerLeft += BindOnPlayerLeave;

        ActivateAllPlayerControllers(); 
        GameManager.Instance.OnLobbyStart(); 
    }

    public override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }
}
