using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
        private State previousState = null; 
        internal PlayerInputManager PlayerInputManager { get; private set; }

        public StateContext(State initialState, PlayerInputManager inputManager)
        {
            PlayerInputManager = inputManager;

            currentState = initialState;
            previousState = currentState;    

            TransitionTo(initialState);
        }

        internal void TransitionTo(State newState) // Action OnStateEnterCallback, Action OnStateExitCallback
        {
            previousState = currentState;
            currentState = newState;

            currentState.SetContext(this);
            GameManager.Instance.SetCurrentState(currentState); 

            Initialize();
        }

        protected void Initialize()
        {
            if (previousState.GetType() != currentState.GetType())
            {
                previousState.OnStateExit();
            }

            currentState.OnStateEnter();
        }

        internal void AddControllerToCurrentState(PlayerController controller)
        {
            currentState.AddPlayerController(controller); 
        }
    }

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

        protected Action OnStateEnterCallback = null;
        protected Action OnStateExitCallback = null;

        public abstract void OnStateEnter();
        public abstract void OnStateExit();


        public void SetContext(StateContext stateContext)
        {
            context = stateContext;
        }

        public void SetCallbacks(Action onStateEnterCallback, Action onStateExitCallback)
        {
            OnStateEnterCallback += onStateEnterCallback;
            OnStateExitCallback += onStateExitCallback; 
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
            GameManager.Instance.SetPlayerRenderer(controller, ActivePlayersControllers.Count);
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

        public void ActivateAllPlayerControllers(bool setPosition = false)
        {
            for (int i = 0; i < ActivePlayersControllers.Count; i++)
            {
                if (ActivePlayersControllers[i] == null)
                {
                    ClearPlayerListFromContext();
                }
            }

            foreach (var item in ActivePlayersControllers)
            {
                Debug.Log("Activating player controller");

                item.ActivateController();
            }
        }

        public void DeactivateAllPlayerControllers()
        {
            foreach (var item in ActivePlayersControllers)
            {
                Debug.Log("Deactivating player controller");

                item.DeactivateController();
            }
        }

        protected void ClearPlayerListFromContext()
        {
            ActivePlayersControllers.Clear();
            ActivePlayersInput.Clear();
        }
    }
}
