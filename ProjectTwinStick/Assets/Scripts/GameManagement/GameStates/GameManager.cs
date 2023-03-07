using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

// Main Menu, Lobby, Game, End Game, Credits
// besoin d'un state Tutorial ? 

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerInputManager playerInputManager;

    public static GameManager Instance;
    public StateContext currentContext;

    [SerializeField, Range(1, 4)] private int playersRequiredAmount = 4;
    private int currentPlayerReadyCount; 

    private void Awake()
    {
        if (Instance)
        {
            Destroy(Instance);
        }
        Instance= this;
    }

    void Start()
    {
        currentContext = new(new LobbyState(), playerInputManager);
        
    }

    public void UpdatePlayerReadyCount(int value)
    {
        currentPlayerReadyCount += value;
        Debug.Log("Updating Player ready count to: " + currentPlayerReadyCount);

        if (currentPlayerReadyCount == playersRequiredAmount)
        {
            currentContext.TransitionTo(new GameState()); 

        }
    }
}
