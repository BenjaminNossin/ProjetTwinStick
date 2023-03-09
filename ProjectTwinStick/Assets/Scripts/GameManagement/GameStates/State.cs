using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Systems.GlobalFramework.States
{
    /// <summary>
    /// This class handles state transition callbacks from the GameManager.
    /// It should only be known by the GameManager class. 
    /// </summary>
    public class StateContext
    {
        private State currentState = null;
        internal PlayerInputManager PlayerInputManager { get; private set; }

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

        internal void AddControllerToCurrentState(PlayerController controller)
        {
            currentState.AddPlayerController(controller); 
        }
    }

    // Main Menu, Lobby, Game, End Game, Credits
    // besoin d'un state Tutorial ?
    // -> plutôt on StateContext tutoriel 
    /// <summary>
    /// States are activated via the StateContext.
    /// They should only be known by the StateContext class.
    /// The states are the only part of the framework that know about PlayerController and Input.
    /// </summary>
    public abstract class State
    {
        protected StateContext context;

        public static List<PlayerInput> ActivePlayersInput = new();
        public static List<PlayerController> ActivePlayersControllers = new();


        public abstract void OnStateEnter();
        public abstract void OnStateExit();

        public void SetContext(StateContext stateContext)
        {
            context = stateContext;
        }

        protected void BindOnPlayerJoined(PlayerInput playerInput)
        {
            Debug.Log("A player joined: " + playerInput);
            ActivePlayersInput.Add(playerInput);
        }

        protected void BindOnPlayerLeave(PlayerInput playerInput)
        {
            Debug.Log("A player left: " + playerInput);
            ActivePlayersInput.Remove(playerInput);
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
}
