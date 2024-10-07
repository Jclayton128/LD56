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


    private void Start()
    {
        StrategicLoopController.Instance.HoneyFactorChanged += HandleHoneyFactorChanged;
        UpgradeController.Instance.PollenHexesToSpendChanged += HandleHexesToSpendChanged;
        HandleHexesToSpendChanged(0);
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

}
