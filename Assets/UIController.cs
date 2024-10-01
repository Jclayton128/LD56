using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    [SerializeField] PanelDriver _introPanel = null;
    [SerializeField] PanelDriver _titlePanel = null;
    [SerializeField] PanelDriver _coreGameLoopPanel = null;
    [SerializeField] PanelDriver _gameOverPanel = null;
    [SerializeField] PanelDriver _creditsPanel = null;
    [SerializeField] PanelDriver _optionsPanel = null;

    /// <summary>
    /// TRUE anytime a tween commanded by this UI Controller is still active. 
    /// Mechanics should reference this in order to prevent changing modes while UI
    /// elements are still doing things.
    /// </summary>
    public bool IsUIActivelyTweening  = false;
    [SerializeField] private float _timeThatTweensWillBeComplete = 0;

    private void Start()
    {
        _introPanel?.InitializePanel(this);
        _titlePanel?.InitializePanel(this);
        _coreGameLoopPanel?.InitializePanel(this);
        _gameOverPanel?.InitializePanel(this);
        _creditsPanel?.InitializePanel(this);
        _optionsPanel?.InitializePanel(this);
    }

    private void Update()
    {
        if (IsUIActivelyTweening)
        {
            if (Time.time >= _timeThatTweensWillBeComplete)
            {
                IsUIActivelyTweening = false;
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
