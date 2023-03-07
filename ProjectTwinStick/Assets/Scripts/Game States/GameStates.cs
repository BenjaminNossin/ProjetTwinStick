using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Main Menu, Lobby, Game, End Game, Credits
// besoin d'un state Tutorial ? 

public class GameStates : MonoBehaviour
{
    // stack of states
    // event on state transition
        // store previous state

    void Start()
    {
        StateContext context = new(new LobbyState()); 
        
    }
}
