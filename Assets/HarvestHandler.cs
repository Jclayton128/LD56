using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HarvestHandler : MonoBehaviour
{
    public Action PollenFactorChanged;

    //refs
    ContextHandler _contextHandler;

    //settings
    [SerializeField] float _pollenCapacity_Starting = 10f;
    [SerializeField] BeeUIDriver _beeUIDriver = null;


    //state
    [SerializeField] List<FlowerHandler> _harvestableFlowersInRange = new List<FlowerHandler>();
    float _pollenCapacity_Current;
    [SerializeField] float _pollenLoad = 0;
    public float PollenLoad => _pollenLoad;
    public float PollenFactor => _pollenLoad / _pollenCapacity_Current;

    [SerializeField] HiveHandler _hiveHandler;

    private void Awake()
    {
        _pollenCapacity_Current = _pollenCapacity_Starting;
        _contextHandler = GetComponent<ContextHandler>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FlowerHandler fh;
        if (collision.TryGetComponent<FlowerHandler>(out fh))
        {
            fh.PollenAvailabilityChanged += HandlePollenAvailabilityChanged;
            if (fh.Pollen > 0)
            {
                if (!_harvestableFlowersInRange.Contains(fh))
                {
                    _harvestableFlowersInRange.Add(fh);
                }
                _contextHandler.AddAvailableContext(ContextHandler.BeeContexts.Harvest);
            }
        }

        HiveHandler hh;
        if (collision.TryGetComponent<HiveHandler>(out hh))
        {
            //TODO check allegiance to make sure that the player is near a friendly hive.
            _hiveHandler = hh;

            if (_pollenLoad > 0)
            {
                _contextHandler.AddAvailableContext(ContextHandler.BeeContexts.DepositPollenAtHive);
            }           
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        FlowerHandler fh;
        if (collision.TryGetComponent<FlowerHandler>(out fh))
        {
            _harvestableFlowersInRange.Remove(fh);
            fh.PollenAvailabilityChanged -= HandlePollenAvailabilityChanged;
            if (_harvestableFlowersInRange.Count == 0)
            {
                _contextHandler.RemoveAvailableContext(ContextHandler.BeeContexts.Harvest);
            }
        }

        HiveHandler hh;
        if (collision.TryGetComponent<HiveHandler>(out hh))
        {
            //TODO check allegiance to make sure that the player is near a friendly hive.
            _hiveHandler = null;
            _contextHandler.RemoveAvailableContext(ContextHandler.BeeContexts.DepositPollenAtHive);
        }

    }

    private void HandlePollenAvailabilityChanged(FlowerHandler flower, bool hasPollenAvailable)
    {
        if (hasPollenAvailable)
        {
            if (!_harvestableFlowersInRange.Contains(flower))
            {
                _harvestableFlowersInRange.Add(flower);
                _contextHandler.AddAvailableContext(ContextHandler.BeeContexts.Harvest);
            }
        }
        else
        {
            _harvestableFlowersInRange.Remove(flower);
            if (_harvestableFlowersInRange.Count == 0)
            {
                _contextHandler.RemoveAvailableContext(ContextHandler.BeeContexts.Harvest);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_contextHandler.BeeContext == ContextHandler.BeeContexts.Harvest &&
                _harvestableFlowersInRange.Count > 0)
            {
                HarvestPollen();
            }

            if ( _contextHandler.BeeContext == ContextHandler.BeeContexts.DepositPollenAtHive &&
                _hiveHandler &&
                _pollenLoad > 0)
            {
                DepositPollen();
            }
        }

    }

    private void HarvestPollen()
    {
        for (int i = _harvestableFlowersInRange.Count -1; i >= 0; i--)
        {
            _pollenLoad += _harvestableFlowersInRange[i].HarvestPollen();
            //_pollenLoad += 1f;
        }
        PollenFactorChanged?.Invoke();
    }

    private void DepositPollen()
    {
        _hiveHandler.DepositPollen(_pollenLoad);
        _pollenLoad = 0;

        _contextHandler.RemoveAvailableContext(ContextHandler.BeeContexts.DepositPollenAtHive);
        PollenFactorChanged?.Invoke();
    }


}
