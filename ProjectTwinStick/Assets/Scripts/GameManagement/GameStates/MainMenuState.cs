using UnityEngine;

namespace Game.Systems.GlobalFramework.States
{
    public class MainMenuState : State
    {
        public override void OnStateEnter()
        {
            Debug.Log("Initializing Main Menu");
            context.PlayerInputManager.onPlayerJoined += BindOnPlayerJoined;
            context.PlayerInputManager.onPlayerLeft += BindOnPlayerLeave;

            ActivateAllPlayerControllers();

            GameManager.Instance.OnMainMenuStart(); 
        }

        public override void OnStateExit()
        {
            
        }
    }
}
