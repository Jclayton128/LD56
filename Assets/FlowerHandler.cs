using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FlowerHandler : MonoBehaviour
{
    public Action<FlowerHandler, bool> PollenAvailabilityChanged;

    //refs
    SpriteRenderer _sr;

    //settings
    [SerializeField] float _pollenRegenRate = 0.1f;
    [SerializeField] float _minPollenForHarvest = 0.2f;
    [SerializeField] float _maxPollenCapacity = 1f;

    //state
    [SerializeField] float _pollen;
    public float Pollen => (_pollen >= _minPollenForHarvest) ? _pollen : 0;
    public float PollenFactor => _pollen / _maxPollenCapacity;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _pollen = UnityEngine.Random.Range(0, _maxPollenCapacity);
    }


    public float HarvestPollen()
    {
        float pollenLoad = _pollen;
        _pollen = 0;
        PollenAvailabilityChanged?.Invoke(this, false);
        return pollenLoad;
    }

    private void Update()
    {
        _pollen += _pollenRegenRate * Time.deltaTime;
        _pollen = Mathf.Clamp(_pollen, 0, _maxPollenCapacity);
        if (_pollen > _minPollenForHarvest)
        {
            PollenAvailabilityChanged?.Invoke(this, true);
        }

        UpdateSpriteFade();
    }


    private void UpdateSpriteFade()
    {
        Color col = Color.white;
        col.a = PollenFactor;
        _sr.color = col;
    }
}
