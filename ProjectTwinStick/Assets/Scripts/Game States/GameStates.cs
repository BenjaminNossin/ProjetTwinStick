using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

// Main Menu, Lobby, Game, End Game, Credits
// besoin d'un state Tutorial ? 

public class GameStates : MonoBehaviour
{
    [SerializeField] private PlayerInputManager playerInputManager;

    public static GameStates Instance;
    public StateContext currentContext; 

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
}
