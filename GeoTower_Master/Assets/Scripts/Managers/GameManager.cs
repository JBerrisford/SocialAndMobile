using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton_Base<GameManager>
{
    public delegate void NewChange();
    public static event NewChange StateChangeEvent;
    public static event NewChange InGameStateChangeEvent;

    protected static CS_Enum.GAME_STATE _gameState;
    public CS_Enum.GAME_STATE GameState
    {
        get { return _gameState; }
    }

    protected static CS_Enum.IN_GAME_STATE _inGameState;
    public CS_Enum.IN_GAME_STATE InGameState
    {
        get { return _inGameState; }
    }

    public static Game_Mode GMode;
    public static Map_Manager mapM;
    public static Player_Manager playerM;
    public static Input_Manager inputM;
    public static Wave_Manager waveM;
    public static AI_Manager aiM;
    public static UI_Manager uiM;

    protected string sceneToLoad;

    void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        DontDestroyOnLoad(gameObject);
        GameModeCreation();
        ManagerCreation();
		NewGameState (CS_Enum.GAME_STATE.MAIN_MENU);
	}

    public void SetSceneToLoad(string sceneName)
    {
        sceneToLoad = sceneName;
    }

    public void NewGameState(CS_Enum.GAME_STATE newState)
    {
        _gameState = newState;
        StateChangeEvent();
    }

    public void NewInGameState(CS_Enum.IN_GAME_STATE newState)
    {
        _inGameState = newState;
        InGameStateChangeEvent();
    }

    void GameModeCreation()
    {
        GMode = transform.Find("GameMode").gameObject.GetComponent<Game_Mode>();
        GMode.Init();
    }

    void ManagerCreation()
    {
        if (mapM == null)
        {
            mapM = transform.Find("MapManager").gameObject.GetComponent<Map_Manager>();
            mapM.Init();
        }

        if (waveM == null)
        {
            waveM = transform.Find("WaveManager").gameObject.GetComponent<Wave_Manager>();
            waveM.Init();
        }

        if (aiM == null)
        {
            aiM = transform.Find("AIManager").gameObject.GetComponent<AI_Manager>();
            aiM.Init();
        }

        if (playerM == null)
        {
            playerM = transform.Find("PlayerManager").gameObject.GetComponent<Player_Manager>();
            playerM.Init();
        }

        if (uiM == null)
        {
            uiM = transform.Find("UIManager").gameObject.GetComponent<UI_Manager>();
            uiM.Init();
        }

        if (inputM == null)
        {
            inputM = transform.Find("InputManager").gameObject.GetComponent<Input_Manager>();
            inputM.Init();
        }
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        NewGameState(CS_Enum.GAME_STATE.START_GAME);
    }

    public override void BeginNewState()
	{
        switch (GameState)
        {
            case CS_Enum.GAME_STATE.MAIN_MENU:
                if(SceneManager.GetActiveScene().name != "MainMenu")
                    SceneManager.LoadScene(0);
                break;
            case CS_Enum.GAME_STATE.NEW:
                SceneManager.LoadScene(sceneToLoad);
                SceneManager.sceneLoaded += SceneLoaded;
                break;
            case CS_Enum.GAME_STATE.LOAD:

                break;
            case CS_Enum.GAME_STATE.SAVE:

                break;
            case CS_Enum.GAME_STATE.START_GAME:

                break;
            case CS_Enum.GAME_STATE.IN_GAME:
                NewInGameState(CS_Enum.IN_GAME_STATE.STANDBY);
                break;
            case CS_Enum.GAME_STATE.GAME_OVER:

                break;
            case CS_Enum.GAME_STATE.EXIT:

                break;
        }
	}

    public override void InGameStateChange()
    {
        switch (InGameState)
        {
            case CS_Enum.IN_GAME_STATE.NOT_ACTIVE:
                Debug.Log("Not_Active");
                break;
            case CS_Enum.IN_GAME_STATE.STANDBY:
                Debug.Log("Standby");
                break;
            case CS_Enum.IN_GAME_STATE.REST_PHASE:
                Debug.Log("Rest_Phase");
                break;
            case CS_Enum.IN_GAME_STATE.START_WAVE:
                Debug.Log("Start_Wave");
                break;
            case CS_Enum.IN_GAME_STATE.END_WAVE:
                Debug.Log("End_Wave");
                break;
            case CS_Enum.IN_GAME_STATE.PAUSED:
                Debug.Log("Pause");
                break;
        }
    }
}


