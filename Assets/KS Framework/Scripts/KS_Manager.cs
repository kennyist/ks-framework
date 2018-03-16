using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void GameStateHandler(KS_Manager.GameState state);
public delegate void VoidHandler();
public delegate void IntHandler(int index);

public class KS_Manager : MonoBehaviour {

    // Instance
    private static KS_Manager instance;
    public static KS_Manager Instance
    {
        get
        {
            return instance;
        }
    }

    // Enums
    public enum GameState
    {
        Intro,
        MainMenu,
        GameMenu,
        LoadScreen,
        Paused,
        Playing
    }

    // Events

    public static event GameStateHandler OnStateChange;
    public static event VoidHandler OnPause;
    public static event VoidHandler OnPlay;
    public static event IntHandler OnLoadLevel;
    public static event VoidHandler OnLevelLoaded;
    public static event VoidHandler OnManagerStart;

    // ------

    private string DefaultConfigPath = "Assets/KS_Data/GameConfig.asset";

    public KS_Scriptable_GameConfig gameConfig;

	// - Private Vars
	private KS_FileHelper fileHelper;
    private KS_Console consoleHelper;
    private ConsoleCommands commands;

    private GameState currentState = GameState.Intro;

    void Awake()
    {
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
    }

    private void Start()
    {
        if(OnManagerStart != null)
        {
            OnManagerStart();
        }
    }

    // Get

    // Game config
    public KS_Scriptable_GameConfig GameConfig { get { return gameConfig; } }
    public string GameName { get { return gameConfig.gameName; } }
    public string Version {  get { return gameConfig.version; } }
    public int Build {  get { return gameConfig.buildNumber; } }

    // Helpers
    public KS_Console Console { get { return consoleHelper; } }
    public KS_FileHelper IO { get { return fileHelper; } }

    // Game Info
    public GameState State { get { return currentState; } }

    // Public Functions

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadLevel(int index)
    {
        SetGameState(GameState.LoadScreen);

        if(OnLoadLevel != null)
        {
            OnLoadLevel(index);
        }
    }

    public void LevelLoaded()
    {
        SetGameState(GameState.Playing);

        if (OnLevelLoaded != null)
            OnLevelLoaded();
    }

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
