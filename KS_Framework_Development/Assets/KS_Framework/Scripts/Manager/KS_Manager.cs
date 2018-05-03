using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core.IO;
using KS_Core.Handlers;
using KS_Core.Console;
using KS_Core.Splash;

namespace KS_Core
{
    /// <summary>
    /// KS game manager. This manages the current state of the game, other global scripts, provides easy methods for game functions and events.
    /// Everything in the gameobject with this script will not be destroyed on loading a level.
    /// </summary>
    public class KS_Manager : MonoBehaviour
    {

        // Instance
        private static KS_Manager instance;
        /// <summary>
        /// Current Active instance of KS_Manager
        /// </summary>
        public static KS_Manager Instance
        {
            get
            {
                return instance;
            }
        }

        // Enums
        /// <summary>
        /// Current Game State
        /// </summary>
        public enum GameState
        {
            /// <summary>
            /// Game is in startup intro stage
            /// </summary>
            Intro,
            /// <summary>
            /// Game is currently in main menus
            /// </summary>
            MainMenu,
            /// <summary>
            /// Game is currently in the pause menu
            /// </summary>
            GameMenu,
            /// <summary>
            /// Game is currently in load screen stage
            /// </summary>
            LoadScreen,
            /// <summary>
            /// Game is currently paused
            /// </summary>
            Paused,
            /// <summary>
            /// Game is currently playing
            /// </summary>
            Playing
        }

        // Events

        /// <summary>
        /// On game state change 
        /// </summary>
        public static event GameStateHandler OnStateChange;
        /// <summary>
        /// On Game Paused
        /// </summary>
        public static event VoidHandler OnPause;
        /// <summary>
        /// On Game Unpaused
        /// </summary>
        public static event VoidHandler OnPlay;
        /// <summary>
        /// On load level called
        /// </summary>
        public static event IntHandler OnLoadLevel;
        /// <summary>
        /// On level fully loaded
        /// </summary>
        public static event VoidHandler OnLevelLoaded;
        /// <summary>
        /// On Manager finish setup
        /// </summary>
        public static event VoidHandler OnManagerStart;
        /// <summary>
        /// On game quit
        /// </summary>
        public static event VoidHandler OnGameQuit;

        // ------

        private string DefaultConfigPath = "Assets/KS_Data/GameConfig.asset";

        /// <summary>
        /// KS framework Game config <see cref="KS_Scriptable_GameConfig"/>
        /// </summary>
        public KS_Scriptable_GameConfig gameConfig;

        /// <summary>
        /// Set the state of the game on launch <see cref="GameState"/>
        /// </summary>
        public GameState startState = GameState.Intro;

        // - Private Vars
        private KS_FileHelper fileHelper;
        private KS_Console consoleHelper;
        private ConsoleCommands commands;

        private GameState currentState = GameState.Intro;

        void Awake()
        {
            DontDestroyOnLoad(this);
            instance = this;
            Debug.Log("Starting OWGF Manager");

            if (!gameConfig)
            {
                Debug.LogError("Game Config not found, Unable to initialize.");
                return;
            }

            // Setup helpers

            fileHelper = new KS_FileHelper(gameConfig);
            consoleHelper = new KS_Console();
            commands = new ConsoleCommands();

            // Saving and loading

            KS_SaveLoad.gameConfig = gameConfig;
        }

        private void Start()
        {
            if (OnManagerStart != null)
            {
                OnManagerStart();
            }
        }

        // Get

        // Game config
        /// <summary>
        /// Get the current game config
        /// </summary>
        public KS_Scriptable_GameConfig GameConfig { get { return gameConfig; } }
        /// <summary>
        /// Get the games name
        /// </summary>
        public string GameName { get { return gameConfig.gameName; } }
        /// <summary>
        /// Get the current game version
        /// </summary>
        public string Version { get { return gameConfig.version; } }
        /// <summary>
        /// Get the current game build
        /// </summary>
        public int Build { get { return gameConfig.buildNumber; } }

        // Helpers
        /// <summary>
        /// Current active console
        /// </summary>
        public KS_Console Console { get { return consoleHelper; } }
        /// <summary>
        /// Current active File Helper
        /// </summary>
        public KS_FileHelper IO { get { return fileHelper; } }

        // Game Info
        /// <summary>
        /// Get the current game state
        /// </summary>
        public GameState State { get { return currentState; } }

        // Public Functions

        /// <summary>
        /// Save the game
        /// </summary>
        /// <param name="name">Save file name</param>
        public void SaveGame(string name)
        {
            KS_SaveLoad.Save(name);
        }

        /// <summary>
        /// Quit the game
        /// </summary>
        public void QuitGame()
        {
            if (OnGameQuit != null)
                OnGameQuit();

            Application.Quit();
        }

        /// <summary>
        /// Load a new level
        /// </summary>
        /// <param name="index">Level index</param>
        public void LoadLevel(int index)
        {
            SetGameState(GameState.LoadScreen);

            if (OnLoadLevel != null)
            {
                OnLoadLevel(index);
            }
        }

        /// <summary>
        /// Set level has been loaded
        /// </summary>
        public void LevelLoaded()
        {
            SetGameState(GameState.Playing);

            if (OnLevelLoaded != null)
                OnLevelLoaded();
        }

        /// <summary>
        /// Set the game state
        /// </summary>
        /// <param name="state">Game state <see cref="GameState"/></param>
        public void SetGameState(GameState state)
        {
            currentState = state;

            Debug.Log("Game State: " + state.ToString());

            switch (state)
            {
                case GameState.Paused:
                    SetPaused(true);
                    break;

                case GameState.Playing:
                    SetPaused(false);
                    break;

                default:
                    break;
            }

            if (OnStateChange != null)
                OnStateChange(state);
        }

        private void SetPaused(bool paused)
        {
            if (paused)
            {
                Time.timeScale = 0f;

                if (OnPause != null)
                    OnPause();
            }
            else
            {
                Time.timeScale = 1f;

                if (OnPlay != null)
                    OnPlay();
            }
        }

    }
}
