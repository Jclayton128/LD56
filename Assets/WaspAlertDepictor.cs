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
        //AUDIO This is called the moment that a wasp detects a bee and has its anger start to charge up. It is turning red while this sound is played.
    }

    private void HandleBecameAngry()
    {
        _moveHandler.SetMoveSpeed(_speedWhenAngry);
        //AUDIO This is called when the wasp has reached full anger and is now shooting off towards the player at high speed.
    }

    private void BecomeCalm()
    {
        _angerTween.Kill();
        _angerTween = _sr.DOColor(Color.white, _timeToBecomeAngry).OnComplete(HandleBecameCalm);
        //AUDIO Called if/when the wasp has lost sight of the player bee and has begun reseting back to normal calm mode where it can move.
    }

    private void HandleBecameCalm()
    {
        _moveHandler.SetMoveSpeed(_normalSpeed);
    }
}
