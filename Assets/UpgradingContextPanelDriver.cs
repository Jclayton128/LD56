using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradingContextPanelDriver : MonoBehaviour
{


    [SerializeField] Image[] _storedHoneyImages = null;
    [SerializeField] Sprite _emptyHoneySprite = null;
    [SerializeField] Sprite _fullHoneySprite = null;

    [SerializeField] TextMeshProUGUI _pollenHexesToSpendTMP = null;

    [SerializeField] GameObject _upgradingPanel = null;
    [SerializeField] GameObject _queenSpeakPanel = null;
    [SerializeField] GameObject _playerSpeakPanel = null;
    [SerializeField] TextMeshProUGUI[] _dialogTMPs = null;

    private void Start()
    {
        StrategicLoopController.Instance.HoneyFactorChanged += HandleHoneyFactorChanged;
        UpgradeController.Instance.PollenHexesToSpendChanged += HandleHexesToSpendChanged;
        HandleHexesToSpendChanged(0);

        _playerSpeakPanel.SetActive(false);
        _queenSpeakPanel.SetActive(false);
        _upgradingPanel.SetActive(false);
        UpgradeController.Instance.PanelModeChanged += HandlePanelModeChanged;
    }

    private void HandleHexesToSpendChanged(int obj)
    {
        _pollenHexesToSpendTMP.text = obj.ToString();
    }

    private void HandleHoneyFactorChanged(int currentHoney, int goalHoney)
    {
        foreach (var honeyImage in _storedHoneyImages)
        {
            honeyImage.sprite = _emptyHoneySprite;
            honeyImage.enabled = false;
        }
        for (int i = 0; i < goalHoney; i++)
        {
            if (i >= _storedHoneyImages.Length)
            {
                Debug.LogWarning("Goal honey amount exceeds UI images to display with.", this);
                break;
            }
            _storedHoneyImages[i].enabled = true;
        }
        for (int i = 0; i < currentHoney; i++)
        {
            if (i >= _storedHoneyImages.Length)
            {
                Debug.LogWarning("Honey invested exceeds UI images to display with.", this);
                break;
            }
            _storedHoneyImages[i].sprite = _fullHoneySprite;
        }
    }

    private void HandlePanelModeChanged()
    {
        switch (UpgradeController.Instance.PanelMode)
        {
            case UpgradeController.PanelModes.PlayerSpeak:
                _playerSpeakPanel.SetActive(true);
                _queenSpeakPanel.SetActive(false);
                _upgradingPanel.SetActive(false);
                //AUDIO This is played when the player is speaking to the queen during the little cutscene upon returning to hive. Maybe a buzzy/talking sound?
                break;

            case UpgradeController.PanelModes.QueenSpeak:
                _playerSpeakPanel.SetActive(false);
                _queenSpeakPanel.SetActive(true);
                _upgradingPanel.SetActive(false);
                //AUDIO This is played when the queen is speaking to the player during the little cutscene upon returning to hive. Maybe a more regal/deeper buzzy/talking sound?
                break;

            case UpgradeController.PanelModes.Upgrade:
                _playerSpeakPanel.SetActive(false);
                _queenSpeakPanel.SetActive(false);
                _upgradingPanel.SetActive(true);
                break;
        }

        foreach (var tmp in _dialogTMPs)
        {
            tmp.text = UpgradeController.Instance.ActivePanelText;
        }
    }

}
