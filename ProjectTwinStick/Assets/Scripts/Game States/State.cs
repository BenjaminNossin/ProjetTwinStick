using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State 
{
    protected StateContext context; 
    public void SetContext(StateContext stateContext)
    {
        context = stateContext; 
    }

    public abstract void Request();
    public abstract void OnStateEnter();
    public abstract void OnStateExit(); 
}
