using UnityEngine;

namespace Game.Systems.GlobalFramework.States
{
    public class OptionsState : State
    {
        public override void OnStateEnter()
        {
            Debug.Log("initializing Options state"); 
        }

        public override void OnStateExit()
        {

        }

    }
}

