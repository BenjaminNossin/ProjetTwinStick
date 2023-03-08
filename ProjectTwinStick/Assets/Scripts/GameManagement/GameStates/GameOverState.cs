using UnityEngine;

public class GameOverState : State
{
    public override void OnStateEnter()
    {
        Debug.Log("Initializing Game Over State");
        DeactivateAllActivePlayerInputs();
    }

    public override void OnStateExit()
    {
        // throw new System.NotImplementedException();
    }
}
