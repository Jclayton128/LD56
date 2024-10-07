using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeeGame;
using System;

public class AttackHandler : MonoBehaviour
{
    Followers _followers;
    ContextHandler _ch;
    EnemyDetector _detector;
    void Start()
    {
        _followers = GetComponentInChildren<Followers>();
        _detector = GetComponentInChildren<EnemyDetector>();
        _ch = GetComponent<ContextHandler>();
        _ch.BeeContextChanged += HandleBeeContextChanged;
        HandleBeeContextChanged();
    }

    private void HandleBeeContextChanged()
    {
        if (_ch.BeeContext == ContextHandler.BeeContexts.Attack)
        {
            enabled = true;
        }
        else enabled = false;
    }

    private void Update()
    {
        if (GameController.Instance.GameMode != GameController.GameModes.Flying) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_followers.NumberOfUsableFollowers > 0)
            {
                _followers.LaunchUnusedFollowerAtTarget(_detector.GetRandomEnemy());

            }
            else
            {
                //TODO click sound of empty chamber... no more bees left to fire
                Debug.Log("No bees left in the chamber!");
            }


        }
    }
}
