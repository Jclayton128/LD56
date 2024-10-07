using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovementHandler : MonoBehaviour
{
    //parameters
    [SerializeField] float _moveSpeed_Starting = 2f;

    [SerializeField] bool _isPlayer = false;

    [Tooltip("Maximum time to keep a bumble")]
    [SerializeField] float _maxBumbleTime = 3f;

    [Tooltip("Minimum time to keep a bumble")]
    [SerializeField] float _minBumbleTime = 1f;

    [Tooltip("Speed Multiplier for Bumble, relative to current move speed")]
    [SerializeField] float _bumbleMultiplier = 1f;

    //state
    [Header("State")]
    [SerializeField] float _moveSpeed_Current;
    public float MoveSpeed => _moveSpeed_Current;
    EnemyHandler _enemyHandler;
    Tween _bumbleTween;
    [SerializeField] Vector2 _bumbleVector = Vector2.zero;
    [SerializeField] Vector2 _desiredVector = Vector2.zero;
    [SerializeField] Vector2 _moveVector = Vector2.zero;

    private void Awake()
    {
        if (!_isPlayer)
        {
            _enemyHandler = GetComponent<EnemyHandler>();
        }
        _moveSpeed_Current = _moveSpeed_Starting;
    }

    private void Start()
    {
        GameController.Instance.GameModeChanged += HandleGameModeChanged;

        _bumbleVector = UnityEngine.Random.insideUnitCircle *
            _bumbleMultiplier;
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

        if (_isPlayer)
        {
            _moveVector =
                (_desiredVector *
                (_moveSpeed_Current + UpgradeController.Instance.SpeedBonus)) 
                +
                (_bumbleVector * _bumbleMultiplier * _moveSpeed_Current);
        }
        else
        {
            _moveVector = (_desiredVector * _moveSpeed_Current) +
                (_bumbleVector * _bumbleMultiplier * _moveSpeed_Current);
        }




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
        if (_isPlayer)
        {
            _desiredVector.x = Input.GetAxis("Horizontal");
            _desiredVector.y = Input.GetAxis("Vertical");
            _desiredVector.Normalize();
        }
        else
        {
            if (_enemyHandler.TargetTransform)
            {
                _desiredVector = (_enemyHandler.TargetTransform.position - transform.position).normalized;
            }
            else
            {
                _desiredVector = Vector2.zero;
            }

        }

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

    public void SetMoveSpeed(float newMoveSpeed)
    {
        _moveSpeed_Current = newMoveSpeed;
    }

    private void OnDestroy()
    {
        GameController.Instance.GameModeChanged -= HandleGameModeChanged;
    }

}
