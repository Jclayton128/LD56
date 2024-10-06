using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeeUIDriver : MonoBehaviour
{
    [SerializeField] Slider _pollenSlider = null;
    [SerializeField] HarvestHandler _harvestHandler = null;

    private void Start()
    {
        _harvestHandler.PollenFactorChanged += HandlePollenChanged;
        HandlePollenChanged();
    }

    private void HandlePollenChanged()
    {
        if (_harvestHandler.PollenFactor <= Mathf.Epsilon)
        {
            _pollenSlider.gameObject.SetActive(false);
        }
        else
        {
            _pollenSlider.gameObject.SetActive(true);
            _pollenSlider.value = _harvestHandler.PollenFactor;
        }

    }

}
