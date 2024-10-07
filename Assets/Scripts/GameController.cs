using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public Action NewGameStarted;



    public enum GameModes { Intro, TitleMenu, Flying, Upgrading, Recruiting,
        GameOver, Credits, Options}
    public Action<GameModes> GameModeChanged;
    public Action<bool> PauseStateChanged_isPaused;

    //settings
    [SerializeField] GameObject _optionMenuReminder = null;
    [SerializeField] float _pollenGoal = 50f;
    public float PollenGoal => _pollenGoal;

    //state
    [SerializeField] bool _isPaused = false;
    GameModes _gameMode = GameModes.Intro;
    public GameModes GameMode => _gameMode;
    bool _hasIntroOccurred = false;
    GameModes _previousGameMode = GameModes.Intro;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UIController.Instance.AllActiveTweensCompleted += HandleAllActiveTweensCompleted;
        Invoke(nameof(Delay_CueIntro), 0.01f);
    }

    private void Delay_CueIntro()
    {
        SetGameMode(GameModes.Intro);
    }

    private void HandleAllActiveTweensCompleted()
    {
        if (!_hasIntroOccurred)
        {
            SetGameMode(GameModes.TitleMenu);
            _hasIntroOccurred = true;
        }
    }


    public void SetGameMode(GameModes newGameMode)
    {

        if (UIController.Instance.IsUIActivelyTweening)
        {
            Debug.Log("Cannot swap game modes with active UI tweening");
            return;
        }
        _previousGameMode = _gameMode;
        _gameMode = newGameMode;

        GameModeChanged?.Invoke(_gameMode);

        if (_gameMode != GameModes.Intro && _gameMode != GameModes.TitleMenu)
        {
            _optionMenuReminder.SetActive(true);
        }
        else
        {
            _optionMenuReminder.SetActive(false);
        }
    }


    public void Handle_NewGamePress()
    {
        SetGameMode(GameModes.Flying);
        NewGameStarted?.Invoke();
    }

    private void Update()
    {
        //ListenForDebug();
        ListenForGameStart();
        ListenForOptionMenu();
    }

    private void ListenForOptionMenu()
    {
        if (Input.GetKeyDown(KeyCode.Tab) &&
            _gameMode != GameModes.Intro &&
            _gameMode != GameModes.TitleMenu)
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        _isPaused = !_isPaused;
        PauseStateChanged_isPaused?.Invoke(_isPaused);
        if (_isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private void ListenForGameStart()
    {
        if (_gameMode == GameModes.TitleMenu)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Handle_NewGamePress();
            }
        } 
    }

    private void ListenForDebug()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetGameMode(GameModes.Intro);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetGameMode(GameModes.TitleMenu);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetGameMode(GameModes.Flying);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetGameMode(GameModes.GameOver);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetGameMode(GameModes.Credits);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SetGameMode(GameModes.Options);
        }


    }

}
