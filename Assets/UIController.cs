using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }
    public Action AllActiveTweensCompleted;
    

    [SerializeField] PanelDriver[] _introPanel = null;
    [SerializeField] PanelDriver[] _titlePanel = null;
    [SerializeField] PanelDriver[] _flyingPanel = null;
    [SerializeField] PanelDriver[] _upgradingPanel = null;
    [SerializeField] PanelDriver[] _recruitingPanel = null;
    [SerializeField] PanelDriver[] _gameOverPanel = null;
    [SerializeField] PanelDriver[] _creditsPanel = null;
    [SerializeField] PanelDriver[] _optionsPanel = null;
    //

    /// <summary>
    /// TRUE anytime a tween commanded by this UI Controller is still active. 
    /// Mechanics should reference this in order to prevent changing modes while UI
    /// elements are still doing things.
    /// </summary>
    public bool IsUIActivelyTweening  = false;
    [SerializeField] private float _timeThatTweensWillBeComplete = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (var panel in _introPanel) panel?.InitializePanel(this);
        foreach (var panel in _titlePanel) panel?.InitializePanel(this);
        foreach (var panel in _flyingPanel) panel?.InitializePanel(this);
        foreach (var panel in _upgradingPanel) panel?.InitializePanel(this);
        foreach (var panel in _recruitingPanel) panel?.InitializePanel(this);
        foreach (var panel in _gameOverPanel) panel?.InitializePanel(this);
        foreach (var panel in _creditsPanel) panel?.InitializePanel(this);
        foreach (var panel in _optionsPanel) panel?.InitializePanel(this);

        //_introPanel?.InitializePanel(this);
        //_titlePanel?.InitializePanel(this);
        //_coreGameLoopPanel?.InitializePanel(this);
        //_gameOverPanel?.InitializePanel(this);
        //_creditsPanel?.InitializePanel(this);
        //_optionsPanel?.InitializePanel(this);

        GameController.Instance.GameModeChanged += HandleGameModeChanged;
        GameController.Instance.PauseStateChanged_isPaused += HandlePauseStateChanged;
    }

    private void HandlePauseStateChanged(bool isPaused)
    {
        if (isPaused) foreach (var panel in _optionsPanel) panel?.ActivatePanel(false);
        else foreach (var panel in _optionsPanel) panel?.RestPanel(false);
    }

    private void HandleGameModeChanged(GameController.GameModes newGameMode)
    {
        switch (newGameMode)
        {
            case GameController.GameModes.Intro:
                foreach (var panel in _introPanel) panel?.ActivatePanel(false);
                foreach (var panel in _titlePanel) panel?.RestPanel(false);
                foreach (var panel in _flyingPanel) panel?.RestPanel(false);
                foreach (var panel in _upgradingPanel) panel?.RestPanel(false);
                foreach (var panel in _recruitingPanel) panel?.RestPanel(false);
                foreach (var panel in _gameOverPanel) panel?.RestPanel(false);
                foreach (var panel in _creditsPanel) panel?.RestPanel(false);
                foreach (var panel in _optionsPanel) panel?.RestPanel(false);
                break;

            case GameController.GameModes.TitleMenu:
                foreach (var panel in _introPanel) panel?.RestPanel(false);
                foreach (var panel in _titlePanel) panel?.ActivatePanel(false);
                foreach (var panel in _flyingPanel) panel?.RestPanel(false);
                foreach (var panel in _upgradingPanel) panel?.RestPanel(false);
                foreach (var panel in _recruitingPanel) panel?.RestPanel(false);
                foreach (var panel in _gameOverPanel) panel?.RestPanel(false);
                foreach (var panel in _creditsPanel) panel?.RestPanel(false);
                foreach (var panel in _optionsPanel) panel?.RestPanel(false);
                break;

            case GameController.GameModes.Flying:
                foreach (var panel in _introPanel) panel?.RestPanel(false);
                foreach (var panel in _titlePanel) panel?.RestPanel(false);
                foreach (var panel in _flyingPanel) panel?.ActivatePanel(false);
                foreach (var panel in _upgradingPanel) panel?.RestPanel(false);
                foreach (var panel in _recruitingPanel) panel?.RestPanel(false);
                foreach (var panel in _gameOverPanel) panel?.RestPanel(false);
                foreach (var panel in _creditsPanel) panel?.RestPanel(false);
                foreach (var panel in _optionsPanel) panel?.RestPanel(false);
                break;


            case GameController.GameModes.Upgrading:
                foreach (var panel in _introPanel) panel?.RestPanel(false);
                foreach (var panel in _titlePanel) panel?.RestPanel(false);
                foreach (var panel in _flyingPanel) panel?.RestPanel(false);
                foreach (var panel in _upgradingPanel) panel?.ActivatePanel(false);
                foreach (var panel in _recruitingPanel) panel?.RestPanel(false);
                foreach (var panel in _gameOverPanel) panel?.RestPanel(false);
                foreach (var panel in _creditsPanel) panel?.RestPanel(false);
                foreach (var panel in _optionsPanel) panel?.RestPanel(false);
                break;


            case GameController.GameModes.Recruiting:
                foreach (var panel in _introPanel) panel?.RestPanel(false);
                foreach (var panel in _titlePanel) panel?.RestPanel(false);
                foreach (var panel in _flyingPanel) panel?.RestPanel(false);
                foreach (var panel in _upgradingPanel) panel?.RestPanel(false);
                foreach (var panel in _recruitingPanel) panel?.ActivatePanel(false);
                foreach (var panel in _gameOverPanel) panel?.RestPanel(false);
                foreach (var panel in _creditsPanel) panel?.RestPanel(false);
                foreach (var panel in _optionsPanel) panel?.RestPanel(false);
                break;

            case GameController.GameModes.GameOver:
                foreach (var panel in _introPanel) panel?.RestPanel(false);
                foreach (var panel in _titlePanel) panel?.RestPanel(false);
                foreach (var panel in _flyingPanel) panel?.RestPanel(false);
                foreach (var panel in _upgradingPanel) panel?.RestPanel(false);
                foreach (var panel in _recruitingPanel) panel?.RestPanel(false);
                foreach (var panel in _gameOverPanel) panel?.ActivatePanel(false);
                foreach (var panel in _creditsPanel) panel?.RestPanel(false);
                foreach (var panel in _optionsPanel) panel?.RestPanel(false);
                break;

            case GameController.GameModes.Credits:
                foreach (var panel in _introPanel) panel?.RestPanel(false);
                foreach (var panel in _titlePanel) panel?.RestPanel(false);
                foreach (var panel in _flyingPanel) panel?.RestPanel(false);
                foreach (var panel in _upgradingPanel) panel?.RestPanel(false);
                foreach (var panel in _recruitingPanel) panel?.RestPanel(false);
                foreach (var panel in _gameOverPanel) panel?.RestPanel(false);
                foreach (var panel in _creditsPanel) panel?.ActivatePanel(false);
                foreach (var panel in _optionsPanel) panel?.RestPanel(false);
                break;

            case GameController.GameModes.Options:
                foreach (var panel in _introPanel) panel?.RestPanel(false);
                foreach (var panel in _titlePanel) panel?.RestPanel(false);
                foreach (var panel in _flyingPanel) panel?.RestPanel(false);
                foreach (var panel in _upgradingPanel) panel?.RestPanel(false);
                foreach (var panel in _recruitingPanel) panel?.RestPanel(false);
                foreach (var panel in _gameOverPanel) panel?.RestPanel(false);
                foreach (var panel in _creditsPanel) panel?.RestPanel(false);
                foreach (var panel in _optionsPanel) panel?.ActivatePanel(false);
                break;
        }
    }

    private void Update()
    {
        if (IsUIActivelyTweening)
        {
            if (Time.time >= _timeThatTweensWillBeComplete)
            {
                IsUIActivelyTweening = false;
                AllActiveTweensCompleted?.Invoke();
            }
        }

    }


    public void SetTweenCompletionTime(float time)
    {
        if (time > _timeThatTweensWillBeComplete && time > Time.time)
        {
            _timeThatTweensWillBeComplete = time;
            IsUIActivelyTweening = true;
        }       
    }



}
