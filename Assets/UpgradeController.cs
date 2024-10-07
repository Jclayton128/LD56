using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    public static UpgradeController Instance { get; private set; }
    public Action PollenCapacityChanged;

    //state
    [SerializeField] int _pollenCap_Starting = 2;

    int _pollenCap_Current;
    public int PollenCap_Current => _pollenCap_Current;

    int _fullHexesToSpend = 0;

    int _upgradeCount_PollenCap = 0;
    int _upgradeCount_PlayerSpeed = 0;
    int _upgradeCount_TypingUpgrade = 0;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameController.Instance.GameModeChanged += HandleGameModeChanged;
        _pollenCap_Current = _pollenCap_Starting;

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
        //Should check for remaining pollen. Do not allow to leave scene until all pollen is spent.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameController.Instance.SetGameMode(GameController.GameModes.Recruiting);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            
        }


    }

    public void BankPollenHexesToSpend(int amountToBank)
    {
        _fullHexesToSpend = amountToBank;
        Debug.Log($"You have {_fullHexesToSpend} in upgrade mode");
    }


}
