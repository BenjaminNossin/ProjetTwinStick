using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Systems.AI;
using UnityEngine.SceneManagement;
using Game.Systems.GlobalFramework.States;
using System.Reflection;

namespace Game.Systems.GlobalFramework
{
    /// <summary>
    /// This Class is in charge of transitions and activation/deactivation of layer 1 gameplay entities (handlers/managers/gameobjects) 
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Game Flow
        [SerializeField] private GameObject mainMenuSelectionsUI;
        [SerializeField] private GameEventTimelineSO tutorialTimeLine;

        [Space, SerializeField] private PlayerInputManager playerInputManager;

        [SerializeField] private List<PlayerRendererLinker> allPlayerRenderers = new List<PlayerRendererLinker>();
        public static GameManager Instance { get; private set; }
        private StateContext currentContext;
        private State initialState = null;
        private Action OnAllPlayersReady; 
        #endregion

        #region Gameplay
        [SerializeField, Range(1, 4)] private int playersRequiredAmount = 4;
        private int currentPlayerReadyCount;
        private readonly List<GameObject> waitRooms = new();

        public GameObject ShipCoreObj { get; private set; }
        private ShipCore shipCore;
        private WaveManager waveManager;
        private GameEventTimelineReader _gameEventTimelineReader;

        [SerializeField] private GameObject gameOverObj;

        public static List<Vector3> spawnPoints = new();
        private int spawnPointIndex;
        public event Action OnGameOverCallBack;
        #endregion

        // NOTE: state stack to avoid new memory allocation when TransitionTo() ?

        private void Awake()
        {
            if (Instance)
            {
                Destroy(Instance);
            }
            Instance = this;
        }

        void Start()
        {
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
            SetObjectActive(gameOverObj, false);

            currentContext = new(new MainMenuState(), playerInputManager);

            //currentContext = new(new LobbyState(), playerInputManager);
        }

        #region Lazy Initializers
        string currentSelected; 
        public void SetSelection(string mainMenuCurrentSelection)
        {
            currentSelected = mainMenuCurrentSelection; 
        }


        public void SetShipCoreData(GameObject obj, ShipCore sC)
        {
            ShipCoreObj = obj;
            shipCore = sC;
        }
        
        public void SetGameEventTimelineReader(GameEventTimelineReader gameEventTimelineReader)
        {
            _gameEventTimelineReader = gameEventTimelineReader;
        }

        public void SetObjectActive(GameObject obj, bool active)
        {
            obj.SetActive(active);
        }

        public void SetPlayerSpawnPosition(PlayerController controller)
        {
            controller.SetControllerSpawnPosition(spawnPoints[spawnPointIndex % playersRequiredAmount]);
            spawnPointIndex++;
        }

        #endregion

        #region Setters
        /// <summary>
        /// Called during the main menu state
        /// </summary>
        /// <param name="requiredState"></param>
        private void SetCurrentSelectedGameState(string requiredState)
        {
            Assembly currentAssembly = Assembly.GetAssembly(typeof(State));
            currentAssembly.CreateInstance(requiredState);
        }

        public void SetPlayerRenderer(PlayerController controller, int index)
        {
            controller.InstantiateRenderer(allPlayerRenderers[index]);
        }
        public void AddControllerToCurrentState(PlayerController controller)
        {
            currentContext.AddControllerToCurrentState(controller); 
        }

        public void AddWaitRoomObj(GameObject obj)
        {
            waitRooms.Add(obj);
            SetObjectActive(obj, false);
        }

        public void AddWaveManager(WaveManager manager)
        {
            waveManager = manager;
        }

        /// <summary>
        /// called from main menu to activate the selected game mode once all players are ready
        /// the player ready count can be dynamically changed (for ex, only one player required to activate credits/options
        /// </summary>
        /// <param name="value">the amount to add to the current players count</param>
        public void UpdatePlayerReadyCount(int value)
        {
            currentPlayerReadyCount += value;
            Debug.Log("Updating Player ready count to: " + currentPlayerReadyCount);

            if (currentPlayerReadyCount == playersRequiredAmount)
            {
                //currentContext.TransitionTo(new GameState());
                SetCurrentSelectedGameState(currentSelected); 
            }
        }

        public void SetAllPlayerSpawnPosition(List<PlayerController> controllers)
        {
            for (int i = 0; i < controllers.Count; i++)
            {
                controllers[i].SetControllerSpawnPosition(spawnPoints[i]);

            }
        }
        #endregion

        #region Callbacks
        public void OnMainMenuStart()
        {
            Debug.Log("starting main menu");
            SetObjectActive(mainMenuSelectionsUI, true);

            foreach (var item in waitRooms)
            {
                SetObjectActive(item, true);
            }
        }

        public void OnMainMenuEnd()
        {
            Debug.Log("ending main menu");
            currentContext.TransitionTo(new LobbyState());
        }

        public void OnLobbyStart()
        {
            SetObjectActive(mainMenuSelectionsUI, false);
        }

        public void OnGameStart()
        {
            foreach (var item in waitRooms)
            {
                SetObjectActive(item, false);
            }

            SetObjectActive(ShipCoreObj, true);

            shipCore.OnGameStart();
            _gameEventTimelineReader.OnGameStart();
            waveManager.OnGameStart();
        }

        public void OnGameEnd()
        {
            SetObjectActive(ShipCoreObj, false);
            SetObjectActive(gameOverObj, true);
           
            _gameEventTimelineReader.OnGameOver();
            waveManager.OnGameOver();
            OnGameOverCallBack?.Invoke();
            currentContext.TransitionTo(new GameOverState());
        }

        public void OnGameQuit()
        {
            SceneManager.LoadSceneAsync("Benji_Test_MainMenu", LoadSceneMode.Single);

        }
        #endregion
    }
}
