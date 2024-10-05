using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveHandler : MonoBehaviour
{
    [SerializeField] float _pollenInHive;

    public void DepositPollen(float pollenToOffload)
    {
        _pollenInHive += pollenToOffload;
        Debug.Log($"Deposited {pollenToOffload}. Now have {_pollenInHive}");
    }
}
