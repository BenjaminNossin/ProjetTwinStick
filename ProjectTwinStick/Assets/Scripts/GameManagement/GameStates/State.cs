using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateContext
{
    private State currentState = null;
    public PlayerInputManager PlayerInputManager { get; private set; }

    public StateContext(State initialState, PlayerInputManager inputManager)
    {
        PlayerInputManager = inputManager;
        TransitionTo(initialState);
    }

    public void TransitionTo(State newState)
    {
        currentState = newState;
        currentState.SetContext(this);
        Initialize(); 
    }

    public void Initialize()
    {
        currentState.OnStateEnter();
    }

    public State GetCurrentState() => currentState;
}

// Main Menu, Lobby, Game, End Game, Credits
// besoin d'un state Tutorial ? 
public abstract class State 
{
    // NOTE: Set Context not very useful for now, remove if that amount of flexibility is not needed
    protected StateContext context;

    protected List<PlayerInput> activePlayersInput = new();
    protected List<PlayerController> activePlayersControllers = new();

    // REFACTOR : State should be the one adding the PlayerController, not the opposite

    public abstract void OnStateEnter();
    public abstract void OnStateExit();

    public void SetContext(StateContext stateContext)
    {
        context = stateContext; 
    }

    protected void BindOnPlayerJoined(PlayerInput playerInput)
    {
        // context.TransitionTo(new GameState());
        Debug.Log("A player joined: " + playerInput);
        activePlayersInput.Add(playerInput);
        // player is spawned
    }

    protected void BindOnPlayerLeave(PlayerInput playerInput)
    {
        Debug.Log("A player left: " + playerInput);
        activePlayersInput.Remove(playerInput);
        // player is despawned
    }

    public void AddPlayerController(PlayerController controller)
    {
        Debug.Log("A player controller was added: " + controller);
        activePlayersControllers.Add(controller);

    }

    public void RemovePlayerController(PlayerController controller)
    {
        Debug.Log("A player controller was removed: " + controller);
        activePlayersControllers.Remove(controller);

    }

    protected void DeactivateAllActivePlayerInputs()
    {
        foreach (var item in activePlayersInput)
        {
            item.DeactivateInput(); 

        }   
    }
}
