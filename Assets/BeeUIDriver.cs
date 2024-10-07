using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BeeUIDriver : MonoBehaviour
{
    [SerializeField] Slider _pollenSlider = null;
    [SerializeField] HarvestHandler _harvestHandler = null;
    [SerializeField] Image[] _pollenImages = null;
    [SerializeField] Sprite[] _fillSprites = null;
    [SerializeField] float _fadeTime = 4f;

    //state
    Tween[] _imageTweens;

    private void Awake()
    {
        _imageTweens = new Tween[_pollenImages.Length];
    }

    private void Start()
    {
        _harvestHandler.PollenLoadChanged += HandlePollenChanged;
        UpgradeController.Instance.PollenCapacityChanged += HandlePollenCapacityChanged;
        HandlePollenChanged(0);
        HandlePollenCapacityChanged();
        foreach (var image in _pollenImages)
        {
            Color col = image.color;
            col.a = 0;
            image.color = col;
        }
    }

    private void HandlePollenCapacityChanged()
    {
        int hexesToDisplay = UpgradeController.Instance.PollenCap_Current;
        
        foreach (var image in _pollenImages)
        {
            image.enabled = false;
        }

        for (int i = 0; i < hexesToDisplay; i++)
        {
            _pollenImages[i].enabled = true;
        }
    }

    private void HandlePollenChanged(int totalQuarters)
    {
        int hexesFilled = totalQuarters / 4;
        int remainder = totalQuarters % 4;

        for (int i = 0; i < _pollenImages.Length; i++)
        {
            _imageTweens[i].Kill();
        }

        foreach (var image in _pollenImages)
        {
            image.color = Color.white;
            image.sprite = _fillSprites[0];
        }

        for (int i = 0; i < hexesFilled; i++)
        {
            _pollenImages[i].sprite = _fillSprites[4];
        }
        
        _pollenImages[hexesFilled].sprite = _fillSprites[remainder];

        for (int i = 0; i < _pollenImages.Length; i++)
        {
            _imageTweens[i] = _pollenImages[i].DOFade(0, _fadeTime).SetEase(Ease.InCubic);
        }

    }



}
