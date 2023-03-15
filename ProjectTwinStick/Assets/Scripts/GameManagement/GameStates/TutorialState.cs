using Game.Systems.GlobalFramework;
using Game.Systems.GlobalFramework.States;
using UnityEngine; 

public class TutorialState : State
{
    public override void OnStateEnter()
    {
        GameManager.Instance.OnTutorialStart(); 
    }

    public override void OnStateExit()
    {
        //
    }
}
