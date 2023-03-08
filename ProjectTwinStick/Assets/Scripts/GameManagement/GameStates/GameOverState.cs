using UnityEngine;

public class GameOverState : State
{
    public override void OnStateEnter()
    {
        Debug.Log("Initializing Game Over State");
        DeactivateAllPlayerControllers();
    }

    public override void OnStateExit()
    {
        // throw new System.NotImplementedException();
    }
}