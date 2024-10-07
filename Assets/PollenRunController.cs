using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeeGame;
using System;

public class PollenRunController : MonoBehaviour
{
    public Action NewPollenRunStarted;

    public static PollenRunController Instance { get; private set; }
    [SerializeField] DayManager _dayManager = null;

    public int FollowerToSpawnOnNextPollenRun { get; set; } = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameController.Instance.GameModeChanged += HandleGameModeChanged;

    }
    private void HandleGameModeChanged(GameController.GameModes newGameMode)
    {
        if (newGameMode == GameController.GameModes.Flying)
        {
            //TODO setup the player for a new pollen run. Drones, pollen load, etc.
            //TODO setup the arena for a safe initial few seconds outside the hive. Move predators away.
            _dayManager.StartDay();
            NewPollenRunStarted?.Invoke();
        }
        else
        {
            _dayManager.StopDay();
        }
    }

}
