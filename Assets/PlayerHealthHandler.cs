using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeeGame;

public class PlayerHealthHandler : HealthHandler
{
    public int HitPoints => _followers.NumberOfUsableFollowers;
    Followers _followers;

    private void Start()
    {
        _followers = transform.parent.GetComponentInChildren<Followers>();
        
    }

    public override void ReduceHitpoints(int hitpoints)
    {
        if (HitPoints == 0)
        {
            Debug.Log("Player just lost last hit point!", this);
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
}
