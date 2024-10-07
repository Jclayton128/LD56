using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeeGame;
using System;

public class PlayerHealthHandler : HealthHandler
{
    public int HitPoints => _followers.NumberOfUsableFollowers;
    Followers _followers;
    HarvestHandler _hh;

    private void Start()
    {
        _hh = transform.parent.GetComponentInChildren<HarvestHandler>();
        _followers = transform.parent.GetComponentInChildren<Followers>();
        UIController.Instance.FadeToBlackCompleted += HandleFadeToBlackCompleted;

    }

    public override void ReduceHitpoints(int hitpoints)
    {
        if (HitPoints == 0)
        {
            Debug.Log("Player just lost last hit point!", this);

            UIController.Instance.FadeToBlack();
            //Destroy(transform.gameObject);
            
        }
        else
        {

            for (int i = 0; i < hitpoints; i++)
            {
                _followers.KillFollowerBeeUponPlayerBeeDamaged();
            }
        }
    }

    private void HandleFadeToBlackCompleted()
    {
        _hh.DumpAllPollen();
        transform.position = new Vector2(0, 0.5f);
        UpgradeController.Instance.ReasonToEnteringMode = UpgradeController.ReasonsForEnteringMode.PlayerDeath;
        GameController.Instance.SetGameMode(GameController.GameModes.Upgrading);
        UIController.Instance.FadeOutFromBlack();
    }
}
