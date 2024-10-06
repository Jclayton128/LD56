using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WaspAlertDepictor : MonoBehaviour
{
    [SerializeField] SpriteRenderer _sr = null;

    //settings
    [SerializeField] float _timeToBecomeAngry = 3f;
    [SerializeField] Color _angryColor = Color.red;
    EnemyHandler _eh;
    Tween _angerTween;
    MovementHandler _moveHandler;
    [SerializeField] float _speedWhenAngry = 5;
    float _normalSpeed;

    private void Start()
    {
        _eh = GetComponentInParent<EnemyHandler>();
        _moveHandler = GetComponentInParent<MovementHandler>();
        _eh.TargetChanged += HandleTargetChanged;
        HandleTargetChanged();
        _normalSpeed = _moveHandler.MoveSpeed;
    }

    private void HandleTargetChanged()
    {
        if (_eh.TargetTransform)
        {
            BecomeAngry();
        }
        else
        {
            BecomeCalm();
        }
    }

    private void BecomeAngry()
    {
        _angerTween.Kill();
        _angerTween = _sr.DOColor(_angryColor, _timeToBecomeAngry).OnComplete(HandleBecameAngry);
        _moveHandler.SetMoveSpeed(0);
    }

    private void HandleBecameAngry()
    {
        _moveHandler.SetMoveSpeed(_speedWhenAngry);
    }

    private void BecomeCalm()
    {
        _angerTween.Kill();
        _angerTween = _sr.DOColor(Color.white, _timeToBecomeAngry).OnComplete(HandleBecameCalm);
    }

    private void HandleBecameCalm()
    {
        _moveHandler.SetMoveSpeed(_normalSpeed);
    }
}
