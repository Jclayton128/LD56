using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspNestHandler : MonoBehaviour
{
    [SerializeField] int _daysBetweenWaspSpawns = 2;
    [SerializeField] GameObject _waspPrefab = null;

    //state
    [SerializeField] int _daysSinceLastWaspSpawn = 0;

    private void Start()
    {
        PollenRunController.Instance.NewPollenRunStarted += HandleNewDay;
        
    }

    private void HandleNewDay()
    {
        if (_daysSinceLastWaspSpawn >= _daysBetweenWaspSpawns)
        {
            SpawnWasp();            
        }
        _daysSinceLastWaspSpawn++;
    }

    private void SpawnWasp()
    {
        Debug.Log("wasp spawned");
        Instantiate(_waspPrefab, transform.position, Quaternion.identity);
        _daysSinceLastWaspSpawn = 0;
    }
}
