using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StrategicLoopController : MonoBehaviour
{
    /// <summary>
    /// current / goal of required honey supply
    /// </summary>
    public Action<int,int> HoneyFactorChanged;

    public static StrategicLoopController Instance { get; private set; }

    [SerializeField] int _honeyGoal = 50;
    public int HoneyGoal => _honeyGoal;

    [SerializeField] int _daysInGame = 10;
    public int DaysElapsed => _daysElapsed;
    public int DaysRemaining => _daysInGame - _daysElapsed;

    //state
    [SerializeField] int _daysElapsed = 0;
    [SerializeField] int _currentHoney = 0;
    public int CurrentHoney => _currentHoney;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PollenRunController.Instance.NewPollenRunStarted += HandleNewPollenRunStarted;
    }

    private void HandleNewPollenRunStarted()
    {
        _daysElapsed++;
        if (_daysElapsed >= _daysInGame)
        {
            //TODO check for strategic win/fail state when 
        }

    }

    public void InvestHoney(int amountToAdd)
    {
        _currentHoney += amountToAdd;
        HoneyFactorChanged?.Invoke(CurrentHoney, HoneyGoal);
    }
}
