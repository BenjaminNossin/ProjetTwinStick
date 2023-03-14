using Game.Systems.GlobalFramework;
using Game.Systems.GlobalFramework.States;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput_MainMenu;

    [SerializeField] private string[] selections;
    [SerializeField] private TMP_Text tmpText; 
    private int currentIndex;

    private static List<Action> actionsCallbacks = new List<Action>();
    private static Action OnAllPlayerReady; 

    private void OnEnable()
    {
        currentIndex = 0;
        UpdateSelection(currentIndex); 

        actionsCallbacks.Add(StartTutorial);
        actionsCallbacks.Add(StartGame);
        actionsCallbacks.Add(ShowOptions);
        actionsCallbacks.Add(ShowCredits);
        actionsCallbacks.Add(QuitGame);

        InputAction toLeft = playerInput_MainMenu.currentActionMap["SelectionLeft"];
        InputAction toRight = playerInput_MainMenu.currentActionMap["SelectionRight"];

        toLeft.performed += ToLeft;
        toRight.performed += ToRight;
    }

    private void ToLeft(InputAction.CallbackContext context)
    {
        Debug.Log("main menu going to left"); 
        UpdateSelection(-1); 
    }

    private void ToRight(InputAction.CallbackContext context)
    {
        Debug.Log("main menu going to right");
        UpdateSelection(1);
    }

    private void UpdateSelection(int updateValue)
    {
        currentIndex += updateValue;
        tmpText.text = selections[Mathf.Abs(currentIndex % selections.Length)];

        GameManager.Instance.SetSelection(tmpText.text); 
    }

    private void StartGame()
    {
        GameManager.Instance.OnMainMenuEnd(); // very BAD from design pattern perspective
    }

    private void QuitGame()
    {
        Debug.Log("Application Quit"); 
        Application.Quit();
    }

    private void ShowCredits()
    {

    }

    private void ShowOptions()
    {

    }

    private void StartTutorial()
    {

    }
}
