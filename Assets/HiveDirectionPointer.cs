using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HiveDirectionPointer : MonoBehaviour
{
    [SerializeField] RectTransform _pointer = null;
    [SerializeField] Image _pointerImage = null;

    //settings
    [SerializeField] float _fadeThreshold = 10f;
    [SerializeField] float _fadeTime = 1.5f;


    //state
    Tween _fadeTween;
    Vector2 _homeHivePos = Vector2.zero;
    Vector2 _beePos;
    Vector2 _dir;
    GameObject _bee;
    [SerializeField] bool _isFaded = false;

    private void Start()
    {
        ArenaController.Instance.NewArenaGenerated += HandleNewArenaGenerated;
        PlayerController.Instance.NewPlayerSpawned += HandleNewPlayerSpawned;
    }

    private void HandleNewPlayerSpawned()
    {
        _bee = PlayerController.Instance.Player;
        MakeFaded();
    }

    private void HandleNewArenaGenerated()
    {
        _homeHivePos = ArenaController.Instance.PlayerHive.transform.position;
    }

    private void Update()
    {
        if (_bee)
        {
            _beePos = _bee.transform.position;
            _dir = (_homeHivePos - _beePos);
        }

        if (!_isFaded && _dir.magnitude <= _fadeThreshold)
        {
            MakeFaded();
        }
        else if (_isFaded && _dir.magnitude > _fadeThreshold)
        {
            MakeVisible();
        }

        _pointer.transform.up = _dir;
    }

    private void MakeVisible()
    {
        _fadeTween.Kill();
        _pointerImage.DOFade(1, _fadeTime);
        _isFaded = false;
    }

    private void MakeFaded()
    {
        _fadeTween.Kill();
        _pointerImage.DOFade(0, _fadeTime);
        _isFaded = true;
    }
}
