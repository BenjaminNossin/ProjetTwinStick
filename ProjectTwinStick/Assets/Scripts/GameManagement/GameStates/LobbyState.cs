using UnityEngine;

namespace Game.Systems.GlobalFramework.States
{
    public class LobbyState : State
    {
        public override void OnStateEnter()
        {
            Debug.Log("Initializing Lobby");
          
            // GameManager.Instance.OnLobbyStart();
        }

        public override void OnStateExit()
        {

        }
    }
}
