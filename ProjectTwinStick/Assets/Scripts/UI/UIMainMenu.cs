using Game.Systems.GlobalFramework;
using Game.Systems.GlobalFramework.States;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public enum MainMenuSelections { Tutorial, MainGame, Options, Credits, Quit, MainMenu }

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private PlayerInputManager inputManager;

    [SerializeField] private MainMenuSelections[] selections = new MainMenuSelections[5];
    [SerializeField] private TMP_Text tmpText; 
    private int currentIndex;
    private MainMenuSelections currentSelection; 

    PlayerInput playerInput_MainMenu;
    InputAction toLeft, toRight; 

    private void OnEnable()
    {
        currentIndex = 0;

        inputManager.onPlayerJoined += GetPlayerInput;
    }

    private void OnDisable()
    {
        Debug.Log("main menu disable"); 
        inputManager.onPlayerJoined -= GetPlayerInput;
        toLeft.performed -= ToLeft;
        toRight.performed -= ToRight;
    }

    private void Start()
    {
        tmpText.text = "Press A to spawn !"; 
    }

    private void GetPlayerInput(PlayerInput firstPlayerToJoin)
    {
        //if (playerInput_MainMenu) return;

        UpdateSelection(currentIndex);

        playerInput_MainMenu = firstPlayerToJoin;

        toLeft = playerInput_MainMenu.currentActionMap["SelectionLeft"];
        toRight = playerInput_MainMenu.currentActionMap["SelectionRight"];

        toLeft.performed += ToLeft;
        toRight.performed += ToRight;

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
        currentIndex %= selections.Length; 
        currentSelection = selections[Mathf.Abs(currentIndex)];

        tmpText.text = $"{currentSelection}";
        if (currentSelection == MainMenuSelections.MainGame)
        {
            tmpText.text = "Main Game"; 
        } // MainMenuSelections.MainGame.ToString().Replace("G"," G"); // bro :DD

        GameManager.Instance.SetCurrentSelectedGameState(currentSelection); 
    }
}
