using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeController : MonoBehaviour
{
    public enum ReasonsForEnteringMode { NormalDeposit, PlayerDeath, NightTime}
    public enum PanelModes { QueenSpeak, PlayerSpeak, Upgrade}

    public Action PanelModeChanged;

    public static UpgradeController Instance { get; private set; }
    public Action PollenCapacityChanged; //STR
    public Action PlayerSpeedChanged; //DEX
    public Action TypingBonusChanged; //CHA or INT
    public Action<int> PollenHexesToSpendChanged;

    [SerializeField] List<Discussion> _depositDiscussions = new List<Discussion>();
    [SerializeField] List<Discussion> _nightDiscussions = new List<Discussion>();
    [SerializeField] List<Discussion> _deathDiscussions = new List<Discussion>();

    [SerializeField] UpgradeOptionPanelDriver _investUp = null;
    [SerializeField] UpgradeOptionPanelDriver _carryDown = null;
    [SerializeField] UpgradeOptionPanelDriver _speedLeft = null;
    [SerializeField] UpgradeOptionPanelDriver _recruitRight = null;

    [SerializeField] TextMeshProUGUI _spaceToAdvance = null;

    //state
    Discussion _activeDiscussion;
    public string ActivePanelText{ get; private set; }
    public ReasonsForEnteringMode ReasonToEnteringMode = 
            ReasonsForEnteringMode.NormalDeposit;

    [SerializeField] PanelModes _panelMode = PanelModes.QueenSpeak;
    public PanelModes PanelMode => _panelMode;

    [SerializeField] int _maxUpgradeLevel = 4;
    [SerializeField] int _pollenCap_Starting = 2;

    int _pollenCap_Current;
    public int PollenCap_Current => _pollenCap_Current;

    [SerializeField] float _speedBonusPerUpgrade = 1f;
    public float SpeedBonus => _speedBonusPerUpgrade * _upgradeCount_PlayerSpeed;

    [SerializeField] float _typingIncrement_PerUpgrade = .09f;
    public float TypingMultiplier =>
        1 - (_typingIncrement_PerUpgrade * _upgradeCount_TypingUpgrade);

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

            EnterDiscussion();
        }
        else enabled = false;
    }

    private void EnterDiscussion()
    {

        int rand;

        switch (ReasonToEnteringMode)
        {
            case ReasonsForEnteringMode.NormalDeposit:
                rand = UnityEngine.Random.Range(0, _depositDiscussions.Count);
                _activeDiscussion = _depositDiscussions[rand];
                break;

            case ReasonsForEnteringMode.PlayerDeath:
                rand = UnityEngine.Random.Range(0, _deathDiscussions.Count);
                _activeDiscussion = _deathDiscussions[rand];
                break;

            case ReasonsForEnteringMode.NightTime:
                rand = UnityEngine.Random.Range(0, _nightDiscussions.Count);
                _activeDiscussion = _nightDiscussions[rand];
                break;


        }

        _activeDiscussion.step = 0;
        ActivePanelText = _activeDiscussion.QueenSpeech0;
        ChangePanelMode(PanelModes.QueenSpeak);

    }

    private void AdvanceDiscussion()
    {
        _activeDiscussion.step++;
        if (_activeDiscussion.step == 1)
        {
            ActivePanelText = _activeDiscussion.PlayerResponse1;
            ChangePanelMode(PanelModes.PlayerSpeak);
        }
        if (_activeDiscussion.step == 2)
        {
            ActivePanelText = _activeDiscussion.QueenResponse2;
            ChangePanelMode(PanelModes.QueenSpeak);
        }
        if (_activeDiscussion.step == 3)
        {
            ActivePanelText = _activeDiscussion.PlayerStinger3;
            ChangePanelMode(PanelModes.PlayerSpeak);
        }
        if (_activeDiscussion.step == 4 || ActivePanelText.Length == 0)
        {
            ChangePanelMode(PanelModes.Upgrade);
        }
    }

    private void ChangePanelMode(PanelModes newPanelMode)
    {
        _panelMode = newPanelMode;
        PushUpgradeOptionsToSubPanels();
        PanelModeChanged?.Invoke();
    }

    private void PushUpgradeOptionsToSubPanels()
    {
        bool invest;
        if (_fullHexesToSpend >= 1) invest = true;
        else invest = false;

        bool capacity;
        if (_fullHexesToSpend >= _upgradeCount_PollenCap + 1) capacity = true;
        else capacity = false;

        bool speed;
        if (_fullHexesToSpend >= _upgradeCount_PlayerSpeed + 1) speed = true;
        else speed = false;

        bool recruit;
        if (_fullHexesToSpend >= _upgradeCount_TypingUpgrade + 1) recruit = true;
        else recruit = false;

        _investUp.SetCost(1, invest);
        _recruitRight.SetCost(_upgradeCount_TypingUpgrade+1, recruit);
        _speedLeft.SetCost(_upgradeCount_PlayerSpeed+1, speed);
        _carryDown.SetCost(_upgradeCount_PollenCap + 1, capacity);

        if (_fullHexesToSpend == 0)
        {
            _spaceToAdvance.enabled = true;
        }
        else
        {
            _spaceToAdvance.enabled = false;
        }
    }

    private void Update()
    {
        if (PanelMode == PanelModes.Upgrade)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_fullHexesToSpend == 0)
                {
                    GameController.Instance.SetGameMode(GameController.GameModes.Recruiting);
                }
                else
                {
                    Debug.LogWarning("Don't allow player to leave until all pollen is spent", this);
                }

            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (_fullHexesToSpend > 0)
                {
                    _fullHexesToSpend--;
                    PollenHexesToSpendChanged?.Invoke(_fullHexesToSpend);
                    StrategicLoopController.Instance.InvestHoney(1);
                    PushUpgradeOptionsToSubPanels();
                }
                else
                {
                    //TODO Play "not enough cash" sound.
                }


            }

            if (Input.GetKeyDown(KeyCode.DownArrow) &&
                _upgradeCount_PollenCap < _maxUpgradeLevel)
            {
                //increase pollen capacity by 1
                if (_fullHexesToSpend >= _upgradeCount_PollenCap + 1)
                {
                    _fullHexesToSpend -= _upgradeCount_PollenCap + 1;
                    PollenHexesToSpendChanged?.Invoke(_fullHexesToSpend);

                    _upgradeCount_PollenCap++;

                    _pollenCap_Current += 1;
                    PollenCapacityChanged?.Invoke();
                    PushUpgradeOptionsToSubPanels();
                    Debug.Log($"Spent {_upgradeCount_PollenCap + 1} to buy {_upgradeCount_PollenCap} level of increased pollen capacity");
                }
                else
                {
                    //TODO Play "not enough cash" sound.
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) &&
                _upgradeCount_PlayerSpeed < _maxUpgradeLevel)
            {
                //Increase move speed
                if (_fullHexesToSpend >= _upgradeCount_PlayerSpeed + 1)
                {
                    _fullHexesToSpend -= _upgradeCount_PlayerSpeed + 1;
                    PollenHexesToSpendChanged?.Invoke(_fullHexesToSpend);

                    _upgradeCount_PlayerSpeed++;
                    PlayerSpeedChanged?.Invoke();
                    PushUpgradeOptionsToSubPanels();
                    Debug.Log($"Spent {_upgradeCount_PlayerSpeed + 1} to buy {_upgradeCount_PlayerSpeed} level of increased move speed");
                }
                else
                {
                    //TODO Play "not enough cash" sound.
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) &&
                _upgradeCount_TypingUpgrade < _maxUpgradeLevel)
            {
                //Fighting? Minigame time help?
                if (_fullHexesToSpend >= _upgradeCount_TypingUpgrade + 1)
                {
                    _fullHexesToSpend -= _upgradeCount_TypingUpgrade + 1;
                    PollenHexesToSpendChanged?.Invoke(_fullHexesToSpend);

                    _upgradeCount_TypingUpgrade++;
                    TypingBonusChanged?.Invoke();
                    PushUpgradeOptionsToSubPanels();
                    Debug.Log($"Spent {_upgradeCount_TypingUpgrade + 1} to buy {_upgradeCount_TypingUpgrade} level of 3rd effect...");
                }
                else
                {
                    //TODO Play "not enough cash" sound.
                }
            }

        }
        if (PanelMode == PanelModes.QueenSpeak ||
            PanelMode == PanelModes.PlayerSpeak)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AdvanceDiscussion();
            }

        }

    }

 

    public void BankPollenHexesToSpend(int amountToBank)
    {
        _fullHexesToSpend = amountToBank;
        PollenHexesToSpendChanged?.Invoke(_fullHexesToSpend);
    }

    [ContextMenu("Debug MakeMoney!")]
    public void Debug_MakeMoney()
    {
        BankPollenHexesToSpend(10);
    }


}
