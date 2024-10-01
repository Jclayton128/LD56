using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System;

public class PanelDriver : MonoBehaviour
{
    //settings    
    [Tooltip("Resting position of the panel")]
    Vector2 _restPosition = Vector2.zero;

    [Tooltip("Active position of the panel")]
    [SerializeField] Vector2 _activePosition = Vector2.zero;

    [Tooltip("How much time for the panel to move to a new position")]
    [SerializeField] float _moveTime = 1f;

    [Tooltip("How much time for the panel to fade in/out")]
    [SerializeField] float _fadeTime = 1f;

    [SerializeField] bool _startsFaded = true;

    //state
    bool _isInitialized = false;
    UIController _uiCon;
    Tween _moveTween;
    RectTransform _rect;
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
        FadeUnfadePanel(_startsFaded, true);
        _restPosition = _rect.anchoredPosition;
        _isInitialized = true;
    }

    private void FindAllImageElements()
    {
        if (!_isInitialized) Debug.LogWarning("Panel has not been initialized!");
        var imageElements = GetComponentsInChildren<Image>();
        foreach (var image in imageElements)
        {
            _imageElements.Add(image, image.color);
            _imageTweens.Add(image, null);
        }
    }

    private void FindAllTextElements()
    {
        var textElements = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var textElement in textElements)
        {
            _textElements.Add(textElement, textElement.color);
            _textTweens.Add(textElement, null);
        }
    }

    public void MovePanelToActivePosition(bool shouldMoveInstantly)
    {
        MovePanel(_activePosition, shouldMoveInstantly);
    }

    public void MovePanelToRestPosition(bool shouldMoveInstantly)
    {
        MovePanel(_restPosition, shouldMoveInstantly);
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
            _rect.DOAnchorPos(destination, _moveTime).SetEase(Ease.InOutQuad);
        }
        PushTweenCompletionTime(_moveTime);
    }

    public void FadeUnfadePanel(bool shouldBeFaded, bool shouldFadeInstantly)
    {
        if (shouldBeFaded)
        {
            foreach (var image in _imageElements.Keys)
            {
                _imageTweens[image].Kill();
                _imageTweens[image] = image.DOFade(0, shouldFadeInstantly ? 0 : _fadeTime);
            }
            foreach (var text in _textElements.Keys)
            {
                _textTweens[text].Kill();
                _textTweens[text] = text.DOFade(0, shouldFadeInstantly ? 0 : _fadeTime);
            }

        }
        else
        {
            foreach (var image in _imageElements.Keys)
            {
                _imageTweens[image].Kill();
                _imageTweens[image] = image.DOFade(1, shouldFadeInstantly ? 0 : _fadeTime);
            }
            foreach (var text in _textElements.Keys)
            {
                _textTweens[text].Kill();
                _textTweens[text] = text.DOFade(1, shouldFadeInstantly ? 0 : _fadeTime);
            }
        }
        PushTweenCompletionTime(shouldFadeInstantly ? 0 : _fadeTime);
    }

    public void ResetPanelToStartCondition(bool shouldResetInstantly)
    {
        MovePanelToRestPosition(shouldResetInstantly);
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




    [ContextMenu("Test Move Off")]
    public void Debug_TestMove1()
    {
        MovePanelToRestPosition(false);
    }


    [ContextMenu("Test Move On")]
    public void Debug_TestMove2()
    {
        MovePanelToActivePosition(false);
    }

    [ContextMenu("Test Fade ")]
    public void Debug_Fade()
    {
        FadeUnfadePanel(true, false);
    }

    [ContextMenu("Test Deade ")]
    public void Debug_Defade()
    {
        FadeUnfadePanel(false, false);
    }

}
