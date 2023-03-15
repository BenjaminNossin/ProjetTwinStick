using Game.Systems.GlobalFramework;
using Game.Systems.GlobalFramework.States;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public enum MainMenuSelections { Tutorial, MainGame, Options, Credits, Quit }

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private PlayerInputManager inputManager; 

    [SerializeField] private MainMenuSelections[] selections = new MainMenuSelections[5];
    [SerializeField] private TMP_Text tmpText; 
    private int currentIndex;
    private MainMenuSelections currentSelection; 

    private static List<Action> actionsCallbacks = new List<Action>();
    PlayerInput playerInput_MainMenu;
    InputAction toLeft, toRight; 

    private void OnEnable()
    {
        currentIndex = 0;

        inputManager.onPlayerJoined += GetPlayerInput;


        actionsCallbacks.Add(StartTutorial);
        actionsCallbacks.Add(StartGame);
        actionsCallbacks.Add(ShowOptions);
        actionsCallbacks.Add(ShowCredits);
        actionsCallbacks.Add(QuitGame);
    }

    private void OnDisable()
    {
        inputManager.onPlayerJoined -= GetPlayerInput;
        toLeft.performed -= ToLeft;
        toRight.performed -= ToRight;
    }

    private void GetPlayerInput(PlayerInput firstPlayerToJoin)
    {
        if (playerInput_MainMenu) return;

        Debug.Log("binding a player input to input actions"); 
        playerInput_MainMenu = firstPlayerToJoin;

        toLeft = playerInput_MainMenu.currentActionMap["SelectionLeft"];
        toRight = playerInput_MainMenu.currentActionMap["SelectionRight"];

        toLeft.performed += ToLeft;
        toRight.performed += ToRight;
    }

    private void Start()
    {
        UpdateSelection(currentIndex);
    }

    private void ToLeft(InputAction.CallbackContext context)
    {
        UpdateSelection(-1); 
    }

    private void ToRight(InputAction.CallbackContext context)
    {
        UpdateSelection(1);
    }

    private void UpdateSelection(int updateValue)
    {
        currentIndex += updateValue;
        currentSelection = selections[Mathf.Abs(currentIndex % selections.Length)]; 
        tmpText.text = $"{currentSelection}";

        GameManager.Instance.SetCurrentSelectedGameState(currentSelection); 
    }


    private void StartTutorial()
    {
        Debug.Log("tutorial");

    }

    private void StartGame()
    {
        GameManager.Instance.OnMainMenuEnd(); // very BAD from design pattern perspective
    }

    private void ShowOptions()
    {
        Debug.Log("options");
    }

    private void ShowCredits()
    {
        Debug.Log("credits"); 
    }

    private void QuitGame()
    {
        Debug.Log("Application Quit");
        Application.Quit();
    }
}
