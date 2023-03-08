using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Systems.AI; 

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerInputManager playerInputManager;

    public static GameManager Instance;
    public StateContext currentContext;

    [SerializeField, Range(1, 4)] private int playersRequiredAmount = 4;
    private int currentPlayerReadyCount; 
    private List<GameObject> waitRooms = new();

    [SerializeField] private GameObject shipCore;
    private WaveManager waveManager;

    [Header("DEBUG")]
    public bool startGameImmediately = false; 

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
        shipCore.SetActive(false); 

        currentContext = new(
            startGameImmediately ? new GameState() : new LobbyState(), 
            playerInputManager);
        
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

    public void AddWaitRoomObj(GameObject obj)
    {
        waitRooms.Add(obj);
    }

    public void AddWaveManager(WaveManager manager)
    {
        waveManager = manager;
    }

    public void OnGameStart()
    {
        foreach (var item in waitRooms)
        {
            item.SetActive(false); 
        }

        shipCore.SetActive(true);
        waveManager.Initialize(); 
    }

    public void OnGameEnd()
    {
        // currentContext.TransitionTo(new )

    }
}
