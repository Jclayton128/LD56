using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlyingContextPanelDriver : MonoBehaviour
{
    //refs
    ContextHandler _contextHandler;
    [SerializeField] TextMeshProUGUI _contextTMP = null;

    private void Start()
    {
        PlayerController.Instance.NewPlayerSpawned += HandleNewPlayerSpawned;
    }

    private void HandleNewPlayerSpawned()
    {
        _contextHandler = PlayerController.Instance.Player.GetComponent<ContextHandler>();
        _contextHandler.BeeContextChanged += HandlePlayerBeeContextChanged;
    }

    private void HandlePlayerBeeContextChanged()
    {
        switch (_contextHandler.BeeContext)
        {
            case ContextHandler.BeeContexts.None:
                _contextTMP.text = " ";
                break;

            case ContextHandler.BeeContexts.Harvest:
                _contextTMP.text = "Space To Harvest";
                break;

            case ContextHandler.BeeContexts.Attack:
                _contextTMP.text = "Space to Attack";
                break;

            case ContextHandler.BeeContexts.DepositPollenAtHive:
                _contextTMP.text = "Space to Deposit Pollen";
                break;
        }
    }
}
