using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollenRunController : MonoBehaviour
{
    public static PollenRunController Instance { get; private set; }

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
        if (newGameMode == GameController.GameModes.Recruiting)
        {
            enabled = true;
        }
        else enabled = false;
    }

    public void PrepareNextPollenRun(int wordsCompletedInMinigame)
    {
        Debug.Log($"Heading out on new Pollen Run with ${wordsCompletedInMinigame} bees in swarm");
        GameController.Instance.SetGameMode(GameController.GameModes.Flying);
        //TODO setup the player for a new pollen run. Drones, pollen load, etc.
        //TODO setup the arena for a safe initial few seconds outside the hive. Move predators away.
    }
}
