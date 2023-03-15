using UnityEngine;

namespace Game.Systems.GlobalFramework.States
{
    public class OptionsState : State
    {
        public override void OnStateEnter()
        {
            GameManager.Instance.OnShowOptions(); 
        }

        public override void OnStateExit()
        {

        }

    }
}

