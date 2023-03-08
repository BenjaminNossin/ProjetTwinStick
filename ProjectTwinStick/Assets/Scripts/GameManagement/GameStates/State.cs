using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class StateContext
{
    private State currentState = null;
    public PlayerInputManager PlayerInputManager { get; private set; }

    public StateContext(State initialState, PlayerInputManager inputManager)
    {
        PlayerInputManager = inputManager;
        TransitionTo(initialState);
    }

    internal void TransitionTo(State newState)
    {
        currentState = newState;
        currentState.SetContext(this);
        Initialize(); 
    }

    protected void Initialize()
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

    public static List<PlayerInput> ActivePlayersInput = new();
    public static List<PlayerController> ActivePlayersControllers = new();

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
        ActivePlayersInput.Add(playerInput);
        // player is spawned
    }

    protected void BindOnPlayerLeave(PlayerInput playerInput)
    {
        Debug.Log("A player left: " + playerInput);
        ActivePlayersInput.Remove(playerInput);
        // player is despawned
    }

    public void AddPlayerController(PlayerController controller)
    {
        Debug.Log("A player controller was added: " + controller);
        ActivePlayersControllers.Add(controller);
        controller.SetUpController();
        controller.ActivateController();
        GameManager.Instance.SetPlayerSpawnPosition(controller); 
    }

    public void RemovePlayerController(PlayerController controller)
    {
        Debug.Log("A player controller was removed: " + controller);
        ActivePlayersControllers.Remove(controller);

    }

    protected void ActivateAllPlayerControllers()
    {
        foreach (var item in ActivePlayersControllers)
        {
            Debug.Log("Activating player controller");

            item.ActivateController();
        }

        GameManager.Instance.SetAllPlayerSpawnPosition(ActivePlayersControllers); 
    }

    protected void DeactivateAllPlayerControllers()
    {
        foreach (var item in ActivePlayersControllers)
        {
            Debug.Log("Deactivating player controller");

            item.DeactivateController();  

        }   
    }
}
