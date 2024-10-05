using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public Action NewGameStarted;

    public enum GameModes { Intro, TitleMenu, CoreGameLoop, GameOver, Credits, Options}
    public Action<GameModes> GameModeChanged;


    //state
    GameModes _gameMode = GameModes.Intro;
    public GameModes GameMode => _gameMode;

    

    private void Awake()
    {
        Instance = this;
    }

    public void SetGameMode(GameModes newGameMode)
    {
        if (UIController.Instance.IsUIActivelyTweening)
        {
            Debug.Log("Cannot swap game modes with active UI tweening");
            return;
        }
        _gameMode = newGameMode;
        GameModeChanged?.Invoke(_gameMode);
    }


    public void Handle_NewGamePress()
    {
        SetGameMode(GameModes.CoreGameLoop);
        NewGameStarted?.Invoke();
    }

    private void Update()
    {
        ListenForDebug();
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
            SetGameMode(GameModes.CoreGameLoop);
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
