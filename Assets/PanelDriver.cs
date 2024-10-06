using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System;

public class PanelDriver : MonoBehaviour
{
    /// <summary>
    /// Used to command panels that have images and TextMeshProUGUIs. Assumption is that
    /// panels are set up how they should be when unused. Anchored position should be inputted
    /// via inspector.
    /// </summary>


    //settings    
    [Tooltip("Resting position of the panel")]
    Vector2 _restPosition = Vector2.zero;

    [Tooltip("Active position of the panel")]
    [SerializeField] Vector2 _activePosition = Vector2.zero;

    [Tooltip("How much time for the panel to move to a new position")]
    [SerializeField] float _moveTime = 1f;

    [Tooltip("How much time for the panel to fade in/out")]
    [SerializeField] float _fadeTime = 1f;

    [SerializeField] bool _startsFaded = false;
    [SerializeField] bool _fadeWhenRested = true;

    //state
    bool _isInitialized = false;
    UIController _uiCon;
    Tween _moveTween;
    RectTransform _rect;
    List<Button> _buttonElements = new List<Button>();
    Dictionary<Image, Color> _imageElements = new Dictionary<Image, Color>();
    Dictionary<TextMeshProUGUI, Color> _textElements = new Dictionary<TextMeshProUGUI, Color>();
    Dictionary<Image, Tween> _imageTweens = new Dictionary<Image, Tween>();
    Dictionary<TextMeshProUGUI, Tween> _textTweens = new Dictionary<TextMeshProUGUI, Tween>();
    float _latestTweenCompletionTime = 0;

    public void InitializePanel(UIController directingUIController)
    {
        _uiCon = directingUIController;
        _rect = GetComponent<RectTransform>();
        FindAllImageElements();
        FindAllTextElements();
        FindAllButtonElements();
        FadeUnfadePanel(_startsFaded, true);
        _restPosition = _rect.anchoredPosition;
        _isInitialized = true;
    }

    private void FindAllImageElements()
    {
        
        var imageElements = GetComponentsInChildren<Image>();
        foreach (var image in imageElements)
        {
            if (image.GetComponent<OverridePanelDriver>()) continue;
            _imageElements.Add(image, image.color);
            _imageTweens.Add(image, null);
        }
    }

    private void FindAllTextElements()
    {
        var textElements = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var textElement in textElements)
        {
            if (textElement.GetComponent<OverridePanelDriver>()) continue;
            _textElements.Add(textElement, textElement.color);
            _textTweens.Add(textElement, null);
        }
    }

    private void FindAllButtonElements()
    {
        var buttonElements = GetComponentsInChildren<Button>();
        foreach (var buttonElement in buttonElements)
        {
            if (buttonElement.GetComponent<OverridePanelDriver>()) continue;
            _buttonElements.Add(buttonElement);
        }
    }

    public void ActivatePanel(bool shouldMoveInstantly)
    {
        gameObject.SetActive(true);
        MovePanel(_activePosition, shouldMoveInstantly);
        ToggleButtons(true);
        if (_fadeWhenRested) FadeUnfadePanel(false, shouldMoveInstantly);
    }

    public void RestPanel(bool shouldMoveInstantly)
    {
        MovePanel(_restPosition, shouldMoveInstantly);
        ToggleButtons(false);
        if (_fadeWhenRested) FadeUnfadePanel(true, shouldMoveInstantly);
        Invoke(nameof(HandleRestPanelCompleted), _moveTime*.99f);
    }

    private void HandleRestPanelCompleted()
    {
        gameObject.SetActive(false);
    }

    private void MovePanel(Vector2 destination, bool shouldMoveInstantly)
    {
        _moveTween.Kill();
        if (shouldMoveInstantly)
        {
            _rect.anchoredPosition = destination;
        }
        else
        {
            _moveTween = _rect.DOAnchorPos(destination, _moveTime).SetEase(Ease.InOutQuad).
                SetUpdate(true);
        }
        PushTweenCompletionTime(_moveTime);
    }

    private void ToggleButtons(bool shouldBeSelectable)
    {
        foreach (var button in _buttonElements)
        {
            button.interactable = shouldBeSelectable;
        }
    }
    private void FadeUnfadePanel(bool shouldBeFaded, bool shouldFadeInstantly)
    {
        if (shouldBeFaded)
        {
            foreach (var image in _imageElements.Keys)
            {
                _imageTweens[image].Kill();
                _imageTweens[image] = image.DOFade(0, shouldFadeInstantly ? 0 : _fadeTime).
                SetUpdate(true); ;
            }
            foreach (var text in _textElements.Keys)
            {
                _textTweens[text].Kill();
                _textTweens[text] = text.DOFade(0, shouldFadeInstantly ? 0 : _fadeTime).
                SetUpdate(true); ;
            }

        }
        else
        {
            foreach (var image in _imageElements.Keys)
            {
                _imageTweens[image].Kill();
                _imageTweens[image] = image.DOFade(1, shouldFadeInstantly ? 0 : _fadeTime).
                SetUpdate(true); ;
            }
            foreach (var text in _textElements.Keys)
            {
                _textTweens[text].Kill();
                _textTweens[text] = text.DOFade(1, shouldFadeInstantly ? 0 : _fadeTime).
                SetUpdate(true); ;
            }
        }
        PushTweenCompletionTime(shouldFadeInstantly ? 0 : _fadeTime);
    }

    public void ResetPanelToStartCondition(bool shouldResetInstantly)
    {
        RestPanel(shouldResetInstantly);
        FadeUnfadePanel(_startsFaded, shouldResetInstantly);
    }

    private void PushTweenCompletionTime(float timeToPush)
    {
        if (Time.time + timeToPush > _latestTweenCompletionTime)
        {
            _latestTweenCompletionTime = Time.time + timeToPush;
        }
        _uiCon.SetTweenCompletionTime(_latestTweenCompletionTime);
    }




    //[ContextMenu("Test Move Off")]
    //public void Debug_TestMove1()
    //{
    //    MovePanelToRestPosition(false);
    //}


    //[ContextMenu("Test Move On")]
    //public void Debug_TestMove2()
    //{
    //    MovePanelToActivePosition(false);
    //}

    //[ContextMenu("Test Fade ")]
    //public void Debug_Fade()
    //{
    //    FadeUnfadePanel(true, false);
    //}

    //[ContextMenu("Test Deade ")]
    //public void Debug_Defade()
    //{
    //    FadeUnfadePanel(false, false);
    //}

}
