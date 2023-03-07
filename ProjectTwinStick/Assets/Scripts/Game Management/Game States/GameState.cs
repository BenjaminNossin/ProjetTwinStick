using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameState : State
{
    public override void OnStateEnter()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnStateExit()
    {
        // throw new System.NotImplementedException();
    }

    public override void Initialize()
    {
        Debug.Log("Initializing Game State");
        GameManager.Instance.OnGameStart(); 
        // init enemy spawner
    }
}
