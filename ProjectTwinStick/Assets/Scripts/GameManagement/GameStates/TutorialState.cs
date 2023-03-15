using Game.Systems.GlobalFramework.States;
using UnityEngine; 

public class TutorialState : State
{
    public override void OnStateEnter()
    {
        Debug.Log("initializing tutorial state"); 
    }

    public override void OnStateExit()
    {
        //
    }
}
