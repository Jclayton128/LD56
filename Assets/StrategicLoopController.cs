using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StrategicLoopController : MonoBehaviour
{
    public enum GameEndType { Success, Failure}
    [SerializeField] Image _endgameImage = null;
    [SerializeField] TextMeshProUGUI _endgameHeaderTMP = null;
    [SerializeField] TextMeshProUGUI _endgameBodyTMP = null;
    [SerializeField] Sprite _successSprite = null;
    [SerializeField] Sprite _failureSprite = null;
    bool _canHitSpace = false;
    [SerializeField] TextMeshProUGUI _hitSpaceToReloadTMP = null;

    /// <summary>
    /// current / goal of required honey supply
    /// </summary>
    public Action<int,int> HoneyFactorChanged;

    public static StrategicLoopController Instance { get; private set; }

    [SerializeField] int _honeyGoal = 50;
    public int HoneyGoal => _honeyGoal;

    [SerializeField] int _totalDaysAllowed = 10;
    public int DaysElapsed => _daysElapsed;
    public int DaysRemaining => _totalDaysAllowed - _daysElapsed;

    //state
    [SerializeField] int _daysElapsed = 0;
    [SerializeField] int _currentHoney = 0;
    public int CurrentHoney => _currentHoney;
    GameEndType _gameEndType = GameEndType.Success;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameController.Instance.GameModeChanged += HandleGameModeChanged;
        PollenRunController.Instance.NewPollenRunStarted += HandleNewPollenRunStarted;
        UIController.Instance.FadeToWhiteCompleted += HandleFadeToWhiteCompleted;
        _hitSpaceToReloadTMP.enabled = false;
    }

    private void Update()
    {
        if (_canHitSpace && Input.GetKeyDown(KeyCode.Space))
        {
            //reload game by reloading scene
            Debug.Log("reloading!");
            SceneManager.LoadScene(0);
        }
    }

    private void HandleGameModeChanged(GameController.GameModes obj)
    {
        if (obj == GameController.GameModes.Upgrading)
        {
            Debug.Log("Checking...");
            if (_daysElapsed >= _totalDaysAllowed)
            {
                Debug.Log("Game Loss!");
                _gameEndType = GameEndType.Failure;
                UIController.Instance.FadeToWhite();
                //TODO check for strategic win/fail state when 
            }
        }

        if (obj == GameController.GameModes.GameOver)
        {
            Invoke(nameof(Delay_GameOver), 7f);            
        }
    }

    private void Delay_GameOver()
    {
        _canHitSpace = true;
        _hitSpaceToReloadTMP.enabled = true;
    }

    private void HandleNewPollenRunStarted()
    {
        _daysElapsed++;
    }

    [ContextMenu("Invest Honey")]
    public void Debug_InvestHoney()
    {
        InvestHoney(1);
    }

    public void InvestHoney(int amountToAdd)
    {
        _currentHoney += amountToAdd;
        HoneyFactorChanged?.Invoke(CurrentHoney, HoneyGoal);

        if (_currentHoney >= _honeyGoal)
        {
            //Game Won!
            Debug.Log("Game won!");
            _gameEndType = GameEndType.Success;
            UIController.Instance.FadeToWhite();
            
        }
    }

    private void HandleFadeToWhiteCompleted()
    {
        SetupEndGamePanel();
        GameController.Instance.SetGameMode(GameController.GameModes.GameOver);
        UIController.Instance.FadeOutFromWhite();
    }

    private void SetupEndGamePanel()
    {
        switch (_gameEndType)
        {
            case GameEndType.Success:
                _endgameHeaderTMP.text = $"Victory!";
                _endgameBodyTMP.text = $"You saved your hive by saving {_currentHoney} honeycombs!";
                _endgameImage.sprite = _successSprite;

                break;

            case GameEndType.Failure:
                _endgameHeaderTMP.text = $"Failure...";
                _endgameBodyTMP.text = $"You only saved {_currentHoney} honeycombs. The guards grimly seal the hive against the icy air and snow outside. Meager rations this winter...";
                _endgameImage.sprite = _failureSprite;
                break;

        }
    }
}
