using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateContext 
{
    private State currentState = null;

    public StateContext(State initialState)
    {
        TransitionTo(initialState); 
    }

    public void TransitionTo(State newState)
    {
        currentState = newState;
        currentState.SetContext(this); 
    }
}

public class GameState : State
{
    public override void OnStateEnter()
    {
        throw new System.NotImplementedException();
        // bind inputs
    }

    public override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }

    public override void Request()
    {
        context.TransitionTo(new LobbyState()); 

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
