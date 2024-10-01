using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

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
        _gameMode = newGameMode;
        GameModeChanged?.Invoke(_gameMode);
    }
}
