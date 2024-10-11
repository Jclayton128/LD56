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
        if (_requestedBeeContexts.Contains(BeeContexts.DepositPollenAtHive))
        {
            SetContext(BeeContexts.DepositPollenAtHive);
            //AUDIO This could be called anytime the hive is close enough to warrant a "Space to Deposit" message on the bottom of the screen. Perhaps the same subtle hint sound that appears with the "space to harvest" message?
        }
        else if(_requestedBeeContexts.Contains(BeeContexts.Attack))
        {
            SetContext(BeeContexts.Attack);
            //AUDIO This is called when the context updates to "Space to Attack". Maybe an ominous alarm-type sound?
        }
        else if (_requestedBeeContexts.Contains(BeeContexts.Harvest))
        {
            SetContext(BeeContexts.Harvest);
            //AUDIO This is called when a harvestable flower enters the range that a Harvest action can be commanded. Maybe a subtle hint that would be called the same instant "Space to Harvest" appears on the screen.
        }
        else
        {
            SetContext(BeeContexts.None);
            //AUDIO If we do sounds for "space for ___" messages, maybe we should include a sound for the context goes back to "no context", which is exactly when the "space to __" message disappears.
        }
    }

    private void SetContext(BeeContexts newBeeContext)
    {
        _beeContext = newBeeContext;        
        BeeContextChanged?.Invoke();
    }
}
