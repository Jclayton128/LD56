using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeeGame;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public Action NewPlayerSpawned;

    //settings
    [SerializeField] GameObject _playerBeePrefab = null;


    //state
    GameObject _player;
    public GameObject Player => _player;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameController.Instance.NewGameStarted += HandleNewGameStarted;
        GameController.Instance.GameModeChanged += HandleGameModeChanged;
    }

    private void HandleGameModeChanged(GameController.GameModes newGameMode)
    {
        if (Player && newGameMode == GameController.GameModes.Flying)
        {
            Player.GetComponentInChildren<Followers>().RespawnFollowers();
        }
    }

    private void HandleNewGameStarted()
    {
        if (_player) Destroy(_player.gameObject);

        _player = Instantiate(_playerBeePrefab, Vector3.zero, Quaternion.identity);
        NewPlayerSpawned?.Invoke();
    }
}
