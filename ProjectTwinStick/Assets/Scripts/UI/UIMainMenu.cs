using Game.Systems.GlobalFramework;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MainMenuSelections { Tutorial, MainGame, Options, Credits, Quit, MainMenu }

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private PlayerInputManager inputManager;

    [SerializeField] private MainMenuSelections[] selections = new MainMenuSelections[5];
    [SerializeField] private TMP_Text tmpText; 
    private int currentIndex;
    private MainMenuSelections currentSelection; 

    PlayerInput playerInput_MainMenu;
    InputAction toLeft, toRight, options, select;

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
        options.performed -= ShowOptions;
        select.performed -= ShowCredits;
    }

    private void Start()
    {
        tmpText.text = "Press a button to spawn !"; 
    }

    private void GetPlayerInput(PlayerInput firstPlayerToJoin)
    {
        UpdateSelection(currentIndex);

        playerInput_MainMenu = firstPlayerToJoin;

        toLeft = playerInput_MainMenu.currentActionMap["SelectionLeft"];
        toRight = playerInput_MainMenu.currentActionMap["SelectionRight"];
        options = playerInput_MainMenu.currentActionMap["Options"];
        select = playerInput_MainMenu.currentActionMap["Credits"];

        toLeft.performed += ToLeft;
        toRight.performed += ToRight;
        options.performed += ShowOptions;
        select.performed += ShowCredits;
    }

    private void ToLeft(InputAction.CallbackContext context)
    {
        UpdateSelection(-1); 
    }

    private void ToRight(InputAction.CallbackContext context)
    {
        UpdateSelection(1);
    }

    private void ShowOptions(InputAction.CallbackContext context)
    {
        GameManager.Instance.OnShowOptions();
    }

    private void ShowCredits(InputAction.CallbackContext context)
    {
        GameManager.Instance.OnShowCredits();
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
        GameManager.Instance.SetAllUIIsActive(false);
    }

}
