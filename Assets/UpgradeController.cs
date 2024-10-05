using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    public static UpgradeController Instance { get; private set; }

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
        if (newGameMode == GameController.GameModes.Upgrading)
        {
            enabled = true;
        }
        else enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameController.Instance.SetGameMode(GameController.GameModes.Recruiting);
        }
    }
}
