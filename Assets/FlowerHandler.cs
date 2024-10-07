using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class FlowerHandler : MonoBehaviour
{
    public enum FlowerTypes { Cone, Zinn, Surprise, Undefined3, Undefined4}

    public Action<FlowerHandler, bool> PollenAvailabilityChanged;

    //refs
    [SerializeField] SpriteRenderer _sr_Stem = null;
    [SerializeField] SpriteRenderer _sr_Flower = null;

    //settings
    [SerializeField] int _daysBetweenReloadingPollen = 2;
    [SerializeField] int _maxPollenCapacity = 1; 
    
    [Tooltip("Maximum time to keep a bumble")]
    [SerializeField] float _maxBumbleTime = 3f;

    [Tooltip("Minimum time to keep a bumble")]
    [SerializeField] float _minBumbleTime = 1f;

    [SerializeField] float _driftMultiplier = 1f;
    [SerializeField] float _fadeTime = 1.5f;


    //state
    int _daysSinceLastHarvest = 0;
    [SerializeField] FlowerTypes _flowerType;
    public FlowerTypes FlowerType => _flowerType;

    [SerializeField] int _pollen;
    public int Pollen => _pollen;

    Tween _moveTween;
    Tween _bloomTween;
    Vector2 _driftVector;

    private void Awake()
    {
        
    }

    private void Start()
    {
        _pollen = _maxPollenCapacity;
        HandleDayPassed();
        //if (_pollen > 0)
        //{
        //    _isHarvestable = true;
        //    Color col = Color.white;
        //    col.a = 1;
        //    _sr_Flower.color = col;
        //}           

        RandomizeSpriteFacings();
        CommenceNewDrift();    
    }

    private void CommenceNewDrift()
    {
        _moveTween.Kill();

        Vector2 newBumbleVec = UnityEngine.Random.insideUnitCircle * _driftMultiplier;
        float timeToHoldNewBumbleVec = UnityEngine.Random.Range(_minBumbleTime, _maxBumbleTime);

        _moveTween = DOTween.To(() =>
        _driftVector, x => _driftVector = x, newBumbleVec, timeToHoldNewBumbleVec).
        SetEase(Ease.InOutSine).
        OnComplete(CommenceNewDrift);
    }

    private void RandomizeSpriteFacings()
    {
        int rand = UnityEngine.Random.Range(0, 4);
        if (rand == 0)
        {
            _sr_Flower.flipX = false;
            _sr_Flower.flipX = false;
        }
        else if (rand == 1)
        {
            _sr_Flower.flipX = true;
            _sr_Flower.flipX = false;
        }
        else if (rand == 2)
        {
            _sr_Flower.flipX = false;
            _sr_Flower.flipX = true;
        }
        else if (rand == 3)
        {
            _sr_Flower.flipX = true;
            _sr_Flower.flipX = true;
        }
    }

    public int HarvestPollen()
    {
        int pollenLoad = _pollen;
        _pollen = 0;
        PollenAvailabilityChanged?.Invoke(this, false);
        FadeFlowerAway();
        //_sr_Flower.enabled = false;
        return pollenLoad;
    }

    private void HandleDayPassed()
    {
        _daysSinceLastHarvest++;
        if (_daysSinceLastHarvest >= _daysBetweenReloadingPollen)
        {
            _pollen = _maxPollenCapacity;
            FadeFlowerIn();
        }
    }

    private void Update()
    {
        _sr_Flower.transform.localPosition = _driftVector;
        //if (GameController.Instance.GameMode != GameController.GameModes.Flying) return;

        //_pollen += _pollenRegenRate * Time.deltaTime;
        //_pollen = Mathf.Clamp(_pollen, 0, _maxPollenCapacity);
        //if (!_isHarvestable && _pollen > _minPollenForHarvest)
        //{
        //    //_sr_Flower.enabled = true;
        //    FadeFlowerIn();
        //    _isHarvestable = true;
        //    PollenAvailabilityChanged?.Invoke(this, true);
        //}



    }

    private void FadeFlowerAway()
    {
        _bloomTween.Kill();
        _bloomTween = _sr_Flower.DOFade(0, _fadeTime);
    }

    private void FadeFlowerIn()
    {
        _bloomTween.Kill();
        _bloomTween = _sr_Flower.DOFade(1, _fadeTime);
    }



}
