using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TutorialController : MonoBehaviour
{
    [SerializeField] List<string> _day0 = null;
    [SerializeField] List<string> _day1 = null;
    [SerializeField] List<string> _day2 = null;
    [SerializeField] List<string> _day3 = null;
    [SerializeField] List<string> _day4 = null;
    [SerializeField] List<string> _day5 = null;
    [SerializeField] List<string> _day6 = null;
    [SerializeField] List<string> _day7 = null;
    [SerializeField] List<string> _day8 = null;
    [SerializeField] List<string> _day9 = null;

    [SerializeField] TextMeshProUGUI _TMP = null;
    [SerializeField] float _timeToDisplay = 5;
    [SerializeField] float _timeToRest = 2f;
    [SerializeField] float _fadeTime = 0.6f;

    List<List<string>> _lore = new List<List<string>>();
    [SerializeField] List<string> _activeDay;
    Tween _tween;
    int _count;
    int _day = -1;

    private void Awake()
    {
        _lore.Add(_day0);
        _lore.Add(_day1);
        _lore.Add(_day2);
        _lore.Add(_day3);
        _lore.Add(_day4);
        _lore.Add(_day5);
        _lore.Add(_day6);
        _lore.Add(_day7);
        _lore.Add(_day8);
        _lore.Add(_day9);
    }

    private void Start()
    {
        GameController.Instance.GameModeChanged += HandleGameModeChanged;
        PollenRunController.Instance.NewPollenRunStarted += HandleNewPollenRun;
        _TMP.DOFade(0, 0.01f);
    }

    private void HandleGameModeChanged(GameController.GameModes obj)
    {
        if (obj != GameController.GameModes.Flying)
        {
            _tween.Kill();
            _activeDay = null;
        }
    }

    private void HandleNewPollenRun()
    {
        _day++;
        _count = 0;
        _activeDay = _lore[_day];
        PushNextText(_activeDay[_count]);
        //_day++;
        //_activeDay = _lore[_day];
    }

    private void HandleNewPlayerSpawned()
    {
        //_day = 0;
        //_activeDay = _lore[_day];
        //PushNextText(_activeDay[_count]);
    }

    private void PushNextText(string text)
    {
        _TMP.text = text;
        _tween.Kill();
        _tween = _TMP.DOFade(1, _fadeTime).OnComplete(TextShown);
    }

    private void TextShown()
    {
        _tween.Kill();
        _tween = _TMP.DOFade(0, _fadeTime).OnComplete(TextHidden).SetDelay(_timeToDisplay);
    }

    private void TextHidden()
    {
        Invoke(nameof(RestCompleted), _timeToRest);
    }

    private void RestCompleted()
    {
        AdvanceToNextText();
    }

    private void AdvanceToNextText()
    {
        _count++;
        if (_count < _activeDay.Count)
        {
            PushNextText(_activeDay[_count]);
        }
    }
}
