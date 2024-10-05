using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HiveHandler : MonoBehaviour
{
    public Action<float> PollenCountChanged;

    [SerializeField] float _pollenInHive;

    public void DepositPollen(float pollenToOffload)
    {
        _pollenInHive += pollenToOffload;
        Debug.Log($"Deposited {pollenToOffload}. Now have {_pollenInHive}");
        PollenCountChanged?.Invoke(_pollenInHive);
    }
}
