using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitmentController : MonoBehaviour
{
    public static RecruitmentController Instance { get; private set; }

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameController.Instance.SetGameMode(GameController.GameModes.Flying);
            //TODO setup the player for a new pollen run. Drones, pollen load, etc.
            //TODO setup the arena for a safe initial few seconds outside the hive. Move predators away.
        }
    }
}
