using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Systems.AI;
using UnityEngine.SceneManagement;
using Game.Systems.GlobalFramework.States;
using UnityEngine.EventSystems;
using TMPro;

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
        [SerializeField] private PlayerInputManager playerInputManager;
        [SerializeField] private List<PlayerRendererLinker> allPlayerRenderers = new List<PlayerRendererLinker>();
        public static GameManager Instance { get; private set; }
        private StateContext currentContext;
        private State currentState = null;
        private Action OnAllPlayersReady;
        private MainMenuSelections mainMenuSelection;
        #endregion

        #region UI
        [Space, SerializeField] private GameObject creditsObj;
        [SerializeField] private GameObject optionsObj;
        [SerializeField] private GameObject SoundSlider;
        [SerializeField] private GameObject gameOverObj;
        [SerializeField] private GameObject gameWonObj;
        [SerializeField] private EventSystem eventSystem;

        [SerializeField] private TMP_Text tmpTimer;

        private bool showOptions, showCredits;
        #endregion

        #region Gameplay

        [Space, SerializeField, Range(1, 4)] private int playersRequiredAmount = 4;
        [Space, SerializeField, Range(1, 15)] private float minutesBeforeWin_Tutorial = 5;
        [Space, SerializeField, Range(1, 20)] private float minutesBeforeWin_MainGame = 12;
        public bool isTutorial { get; private set; }
        private bool playing; 
        private float timeBeforeWin_AsSeconds;
        private float currentPlayTime; 

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

        #region Audio
        [SerializeField] AudioSource audiosource;
        [SerializeField] AudioClip lobbyMusic;
        [SerializeField] AudioClip[] playList;
        AudioClip lastsong;
        #endregion

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

            currentContext = new(new LobbyState(), playerInputManager);
            Invoke(nameof(InitializeContext), 0.2f);
        }

        float min, sec;
        private void Update()
        {
            if (playing)
            {
                min = Mathf.FloorToInt(timeBeforeWin_AsSeconds / 60f); 
                sec = timeBeforeWin_AsSeconds % 60f;

                var span = new TimeSpan(0, 0, (int)timeBeforeWin_AsSeconds); //Or TimeSpan.FromSeconds(seconds); (see Jakob C�s answer)
                var timeFormatted = string.Format("{0}:{1:00}",
                                            (int)span.TotalMinutes,
                                            span.Seconds);

                tmpTimer.text = $"{timeFormatted}";

                timeBeforeWin_AsSeconds -= Time.deltaTime;

            }

            if (playing || isTutorial) 
            {
                if (!audiosource.isPlaying) PlayGameMusic();
            } 

        }

        private void InitializeContext()
        {
            spawnPointIndex = 0;

            currentContext.TransitionTo(currentState = new MainMenuState());
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

            if (currentPlayerReadyCount == playersRequiredAmount)
            {
                currentContext.TransitionTo(currentState = GetStateFromFactory(mainMenuSelection));
            }
        }
             
        public void SetAllPlayerSpawnPosition(List<PlayerController> controllers)
        {
            for (int i = 0; i < controllers.Count; i++)
            {
                controllers[i].SetControllerSpawnPosition(spawnPoints[i]);
            }
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

        private void DeactivateAllGameplayObjects()
        {
            Debug.Log("deactivating all gameplay objects"); 
            playing = false; 

            SetObjectActive(ShipCoreObj, false);
            _gameEventTimelineReader.OnGameOver();
            waveManager.OnGameOver();
            OnGameOverCallBack?.Invoke();
        }

        #endregion

        #region ""Callbacks"" ;))

        public void ReloadContext()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }

        public void OnMainMenuStart()
        {
            Debug.Log("starting main menu");
            CancelInvoke(nameof(OnGameWin));

            currentPlayerReadyCount = 0;
            //spawnPointIndex = 0;
            isTutorial = false;

            SetObjectActive(mainMenuSelectionsUI, true);

            foreach (var item in waitRooms)
            {
                SetObjectActive(item, true);
            }

            audiosource.loop = true;
            audiosource.clip = lobbyMusic;
            audiosource.Play();
        }

        public void OnMainMenuEnd()
        {
            
            Debug.Log("ending main menu");
        }

        public void OnLobbyStart()
        {
        }

        void PlayGameMusic()
        {
            int rand = UnityEngine.Random.Range(0, playList.Length - 1);
            lastsong = playList[rand];
            AudioClip permute = playList[playList.Length - 1];
            audiosource.clip = lastsong;
            audiosource.Play();
            playList[rand] = permute;
            playList[playList.Length - 1] = lastsong;
        }
        public void OnTutorialStart()
        {
            audiosource.Stop();
            audiosource.loop = false;
            isTutorial = true;

            _gameEventTimelineReader.SetNewTimeline(tutorialTimeLine);

            currentContext.TransitionTo(currentState = new GameState());
            audiosource.clip = playList[UnityEngine.Random.Range(0, playList.Length)];
            audiosource.Play();
        }

        public void OnGameStart()
        {
            audiosource.Stop();
            audiosource.loop = false;
            Debug.Log("Starting Game. Is tutorial: " + isTutorial);
            playing = true; 

            timeBeforeWin_AsSeconds = (isTutorial ? minutesBeforeWin_Tutorial : minutesBeforeWin_MainGame) * 60f; 
            Invoke(nameof(OnGameWin), timeBeforeWin_AsSeconds); 


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

        public void OnGameOver()
        {
            audiosource.Stop();
            CancelInvoke(nameof(OnGameWin));

            SetObjectActive(gameOverObj, true);

            DeactivateAllGameplayObjects();
            currentContext.TransitionTo(currentState = new GameOverState());
        }

        public void OnShowOptions()
        {
            if (playing) return;

            Debug.Log("options");
            showOptions = !showOptions;
            showCredits = false;

            currentState.DeactivateAllPlayerControllers();

            SetObjectActive(optionsObj, showOptions);
            SetObjectActive(creditsObj, showCredits);

            SetObjectActive(mainMenuSelectionsUI, !(showOptions || showCredits));
            eventSystem.SetSelectedGameObject(SoundSlider);
            

            if (!(showOptions || showCredits))
            {
                currentState.ActivateAllPlayerControllers();
            }
        }

        public void OnShowCredits()
        {
            if (playing) return; 

            Debug.Log("credits");
            showCredits = !showCredits;
            showOptions = false;

            currentState.DeactivateAllPlayerControllers();

            SetObjectActive(creditsObj, showCredits);
            SetObjectActive(optionsObj, showOptions);

            SetObjectActive(mainMenuSelectionsUI, !(showOptions || showCredits));

            if (!(showOptions || showCredits))
            {
                currentState.ActivateAllPlayerControllers();
            }
        }

        public void OnGameWin()
        {
            SetObjectActive(gameWonObj, true);

            DeactivateAllGameplayObjects();
            currentContext.TransitionTo(currentState = new WinState());
        }

        public void OnGameQuit()
        {
            Debug.Log("Application Quit");
            Application.Quit();
        }

        #endregion

        #region UI


        private void HideMainMenuSelection()
        {
            mainMenuSelectionsUI.SetActive(false);
        }

        public void HideTimer()
        {
            tmpTimer.text = string.Empty;
        }

        #endregion
    }
}