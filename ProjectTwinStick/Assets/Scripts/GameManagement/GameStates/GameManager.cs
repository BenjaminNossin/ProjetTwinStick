using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Systems.AI;
using UnityEngine.SceneManagement;

/// <summary>
/// This Class is in charge of state transitions and activation/deactivation of global entities
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerInputManager playerInputManager;

    public static GameManager Instance;
    private StateContext currentContext; 

    [SerializeField, Range(1, 4)] private int playersRequiredAmount = 4;
    private int currentPlayerReadyCount; 
    private readonly List<GameObject> waitRooms = new();

    [SerializeField] private GameObject shipCoreObj;
    private ShipCore shipCore;
    private WaveManager waveManager;

    [SerializeField] private GameObject gameOverObj; 
    private UIGameOver gameOverUI;

    public static List<Vector3> spawnPoints = new();
    private int spawnPointIndex; 


    // TODO: state stack to avoid new memory allocation when TransitionTo()

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
        gameOverUI = gameOverObj.GetComponent<UIGameOver>();
        shipCore = shipCoreObj.GetComponent<ShipCore>();

        for (int i = 0; i < playerInputManager.transform.childCount; i++)
        {
            spawnPoints.Add(playerInputManager.transform.GetChild(i).position); 
        }

        Initialize();
    }

    public void Initialize()
    {
        currentPlayerReadyCount = 0;
        spawnPointIndex = 0;

        shipCoreObj.SetActive(false);
        gameOverObj.SetActive(false);

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

    public void AddWaitRoomObj(GameObject obj)
    {
        waitRooms.Add(obj);
    }

    public void AddWaveManager(WaveManager manager)
    {
        waveManager = manager;
    }

    public void SetPlayerSpawnPosition(PlayerController controller)
    {
        controller.SetControllerSpawnPosition(spawnPoints[spawnPointIndex % playersRequiredAmount]);
        spawnPointIndex++; 
    }

    public void SetAllPlayerSpawnPosition(List<PlayerController> controllers)
    {
        for (int i = 0; i < controllers.Count; i++)
        {
            controllers[i].SetControllerSpawnPosition(spawnPoints[i]); 

        }
    }

    public void OnLobbyStart()
    {
        foreach (var item in waitRooms)
        {
            item.SetActive(true);
        }
    }

    public void OnGameStart()
    {
        foreach (var item in waitRooms)
        {
            item.SetActive(false);
        }

        shipCoreObj.SetActive(true);
        shipCore.OnGameStart(); 
        waveManager.OnGameStart(); 
    }

    public void OnGameEnd()
    {
        waveManager.OnGameOver();

        shipCoreObj.SetActive(false);
        gameOverObj.SetActive(true);

        currentContext.TransitionTo(new GameOverState());
    }

    public void OnGameQuit()
    {
        SceneManager.LoadSceneAsync("Benji_Test_MainMenu", LoadSceneMode.Single); 

    }

    public State GetCurrentState() => currentContext.GetCurrentState(); 
}
