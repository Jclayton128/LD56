using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovementHandler : MonoBehaviour
{
    //parameters
    [SerializeField] float _moveSpeed = 2f;


    [Tooltip("Maximum time to keep a bumble")]
    [SerializeField] float _maxBumbleTime = 3f;

    [Tooltip("Minimum time to keep a bumble")]
    [SerializeField] float _minBumbleTime = 1f;

    [Tooltip("Multiplier for bumble vector. Should be less than MoveSpeed")]
    [SerializeField] float _bumbleSpeed = 1f;

    //state
    Tween _bumbleTween;
    Tween _bumbleTweenY;

    [SerializeField] Vector2 _bumbleVector = Vector2.zero;
    [SerializeField] Vector2 _desiredVector = Vector2.zero;
    [SerializeField] Vector2 _moveVector = Vector2.zero;

    private void Start()
    {
        GameController.Instance.GameModeChanged += HandleGameModeChanged;

        _bumbleVector = UnityEngine.Random.insideUnitCircle *
            _bumbleSpeed;
        CommenceNewBumbling();
    }

    private void CommenceNewBumbling()
    {
        _bumbleTween.Kill();

        Vector2 newBumbleVec = UnityEngine.Random.insideUnitCircle;
        float timeToHoldNewBumbleVec = UnityEngine.Random.Range(_minBumbleTime, _maxBumbleTime);

        _bumbleTween = DOTween.To(() =>
        _bumbleVector, x => _bumbleVector = x, newBumbleVec, timeToHoldNewBumbleVec).
        SetEase(Ease.InOutElastic).
        OnComplete(CommenceNewBumbling);

    }

    private void Update()
    {
        UpdateDesiredVector();
        _moveVector = (_desiredVector * _moveSpeed) + (_bumbleVector * _bumbleSpeed);

        UpdatePosition();


        //Debug.DrawLine(transform.position, transform.position + (Vector3)_bumbleVector,
        //    Color.blue);
        //Debug.DrawLine(transform.position, transform.position + (Vector3)_desiredVector,
        //    Color.yellow);        
        //Debug.DrawLine(transform.position, transform.position + (Vector3)_moveVector,
        //    Color.green);

    }

    private void UpdateDesiredVector()
    {
        _desiredVector.x = Input.GetAxis("Horizontal");
        _desiredVector.y = Input.GetAxis("Vertical");
        _desiredVector.Normalize();
    }

    private void UpdatePosition()
    {
        transform.position += (Vector3)_moveVector * Time.deltaTime;
    }


    private void HandleGameModeChanged(GameController.GameModes newGameMode)
    {
        if (newGameMode == GameController.GameModes.Flying)
        {
            enabled = true;
        }
        else enabled = false;
    }


}
