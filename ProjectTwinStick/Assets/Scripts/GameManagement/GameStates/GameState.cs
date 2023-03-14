using UnityEngine;

namespace Game.Systems.GlobalFramework.States
{
    public class GameState : State
    {
        public override void OnStateEnter()
        {
            Debug.Log("Initializing Game State");
            GameManager.Instance.OnGameStart();
        }

        public override void OnStateExit()
        {

        }
    }
}
