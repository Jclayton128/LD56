using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextHandler : MonoBehaviour
{
    public Action BeeContextChanged;
    public enum BeeContexts {None, Harvest, Attack, DepositPollenAtHive}


    //state
    BeeContexts _beeContext = BeeContexts.None;
    public BeeContexts BeeContext => _beeContext;

    [SerializeField] List<BeeContexts> _requestedBeeContexts = new List<BeeContexts>();

    private void Start()
    {
        ReassessRequestedContexts();
    }

    public void AddAvailableContext(BeeContexts newContext)
    {
        if (!_requestedBeeContexts.Contains(newContext))
        {
            _requestedBeeContexts.Add(newContext);
        }
        ReassessRequestedContexts();
    }



    public void RemoveAvailableContext(BeeContexts expiredContext)
    {
        if (_requestedBeeContexts.Contains(expiredContext))
        {
            _requestedBeeContexts.Remove(expiredContext);
        }
        ReassessRequestedContexts();
    }

    private void ReassessRequestedContexts()
    {
        if (_requestedBeeContexts.Contains(BeeContexts.Attack))
        {
            SetContext(BeeContexts.Attack);
        }
        else if (_requestedBeeContexts.Contains(BeeContexts.Harvest))
        {
            SetContext(BeeContexts.Harvest);
        }
        else if (_requestedBeeContexts.Contains(BeeContexts.DepositPollenAtHive))
        {
            SetContext(BeeContexts.DepositPollenAtHive);
        }
        else
        {
            SetContext(BeeContexts.None);
        }
    }

    private void SetContext(BeeContexts newBeeContext)
    {
        _beeContext = newBeeContext;        
        BeeContextChanged?.Invoke();
    }
}
