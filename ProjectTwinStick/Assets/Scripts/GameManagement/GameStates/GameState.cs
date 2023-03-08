using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameState : State
{
    public override void OnStateEnter()
    {
        Debug.Log("Initializing Game State");
        GameManager.Instance.OnGameStart(); 
    }

    public override void OnStateExit()
    {
        // throw new System.NotImplementedException();
    }
}
