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
        private MainMenuSelections mainMenuSelection;
        #endregion

        #region UI
        [SerializeField] private GameObject creditsObj;
        [SerializeField] private GameObject optionsObj;
        [SerializeField] private GameObject gameOverObj;
        #endregion

        #region Gameplay
        [SerializeField, Range(1, 4)] private int playersRequiredAmount = 4;
        private int currentPlayerReadyCount;
        private readonly List<GameObject> waitRooms = new();

        public GameObject ShipCoreObj { get; private set; }
        private ShipCore shipCore;
        private WaveManager waveManager;
        private GameEventTimelineReader _gameEventTimelineReader;

        public static List<Vector3> spawnPoints = new();
        private int spawnPointIndex;
        public event Action OnGameOverCallBack;
        public event Action OnGameStartCallback;
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

            Invoke(nameof(Initialize), 0.2f); 
        }

        public void Initialize()
        {
            currentPlayerReadyCount = 0;
            spawnPointIndex = 0;

            SetAllUIIsActive(false);
            currentContext = new(new MainMenuState(), playerInputManager);
        }

        #region Lazy Initializers
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
        public void SetCurrentSelectedGameState(MainMenuSelections requiredState)
        {
            mainMenuSelection = requiredState; 
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
            Debug.Log("adding wait room"); 
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

            SetAllUIIsActive(false); 
            if (currentPlayerReadyCount == playersRequiredAmount)
            {
                currentContext.TransitionTo(GetStateFromFactory(mainMenuSelection));
            }
        }

        public void SetAllPlayerSpawnPosition(List<PlayerController> controllers)
        {
            for (int i = 0; i < controllers.Count; i++)
            {
                controllers[i].SetControllerSpawnPosition(spawnPoints[i]);

            }
        }

        public void SetNewState(MainMenuSelections mms)
        {
            currentContext.TransitionTo(GetStateFromFactory(mms));
        }

        private static State GetStateFromFactory(MainMenuSelections mms) =>
            mms switch
            {
                MainMenuSelections.Tutorial => new TutorialState(),
                MainMenuSelections.MainGame => new GameState(),
                MainMenuSelections.Options => new OptionsState(),
                MainMenuSelections.Credits => new CreditsState(),
                MainMenuSelections.Quit => new QuitState(),
                MainMenuSelections.MainMenu => new MainMenuState(),
                _ => throw new ArgumentException("Invalid enum value for main menu selections", nameof(mms)),
            }; 
        

        #endregion

        #region Callbacks
        public void ReloadContext()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }

        public void OnMainMenuStart()
        {
            Debug.Log("starting main menu");
            SetObjectActive(mainMenuSelectionsUI, true);

            foreach (var item in waitRooms)
            {
                Debug.Log("wait room activated"); 
                SetObjectActive(item, true);
            }
        }

        public void OnMainMenuEnd()
        {
            Debug.Log("ending main menu");
            //currentContext.TransitionTo(new LobbyState());
        }

        public void OnLobbyStart()
        {
            //SetObjectActive(mainMenuSelectionsUI, false);
        }

        public void OnTutorialStart()
        {
            Debug.Log("tutorial");
            SetAllUIIsActive(false);
            _gameEventTimelineReader.SetNewTimeline(tutorialTimeLine);

            currentContext.TransitionTo(new GameState());
        }

        public void OnGameStart()
        {
            SetAllUIIsActive(false);

            foreach (var item in waitRooms)
            {
                SetObjectActive(item, false);
            }

            SetObjectActive(mainMenuSelectionsUI, false);
            SetObjectActive(ShipCoreObj, true);
            shipCore.OnGameStart();
            _gameEventTimelineReader.OnGameStart();
            waveManager.OnGameStart();
            OnGameStartCallback?.Invoke();
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

        public void OnShowOptions()
        {
            Debug.Log("options");
            optionsObj.SetActive(true);
            creditsObj.SetActive(false);
        }

        public void OnShowCredits()
        {
            Debug.Log("credits");
            creditsObj.SetActive(true);
            optionsObj.SetActive(false);
        }

        public void OnGameQuit()
        {
            Debug.Log("Application Quit");
            SetAllUIIsActive(false);
            Application.Quit();

        }
        #endregion

        #region UI
        private void SetAllUIIsActive(bool isActive)
        {
            gameOverObj.SetActive(isActive); 
            optionsObj.SetActive(isActive);
            creditsObj.SetActive(isActive);
        }
        #endregion
    }
}
