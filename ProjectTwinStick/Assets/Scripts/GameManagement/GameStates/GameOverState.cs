using UnityEngine;

namespace Game.Systems.GlobalFramework.States
{
    public class GameOverState : State
    {
        public override void OnStateEnter()
        {
            Debug.Log("Initializing Game Over State");
            DeactivateAllPlayerControllers();
            ClearPlayerListFromContext();
        }

        public override void OnStateExit()
        {

        }
    }
}
