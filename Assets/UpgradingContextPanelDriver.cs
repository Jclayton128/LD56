using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradingContextPanelDriver : MonoBehaviour
{
    [SerializeField] Slider _pollenSlider;



    private void Start()
    {
        ArenaController.Instance.NewArenaGenerated += HandleNewArenaGenerated;
        _pollenSlider.maxValue = GameController.Instance.PollenGoal;
    }

    private void HandleNewArenaGenerated()
    {
        ArenaController.Instance.PlayerHive.PollenCountChanged += HandlePollenCountChanged;
    }

    private void HandlePollenCountChanged(float currentPollenInHive)
    {
        _pollenSlider.value = currentPollenInHive;
    }
}
