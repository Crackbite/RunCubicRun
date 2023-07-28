using DG.Tweening;
using Lean.Localization;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrainingPhraseDisplay : MonoBehaviour
{
    [SerializeField] private LeanLocalizedTextMeshProUGUI _localizedText;
    [SerializeField] private List<GameObject> _phrases;
    [SerializeField] private LeanLocalization _localization;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _defaultSpacing = 10f;
    [SerializeField] private float _russianSpacing = 0f;

    private string _currentLanguage;
    private readonly string RussianLanguage = "Russian";
    private bool _isDisplayStop;
    Tweener _tweener = null;

    const float OpacityValue = 1.0f;
    const float FadeDuration = 1.0f;

    public event Action DisplayCompleted;

    public int PhrasesAmount { get; private set; }

    private void Start()
    {
        PhrasesAmount = _phrases.Count;
        _currentLanguage = _localization.CurrentLanguage;

        if (_currentLanguage == RussianLanguage)
        {
            _text.characterSpacing = _russianSpacing;
        }
        else
        {
            _text.characterSpacing = _defaultSpacing;
        }
    }

    public void Display(int _nextPhraseNumber)
    {
        string emptyPhrase = "";

        _localizedText.TranslationName = _phrases[_nextPhraseNumber] != null ? _phrases[_nextPhraseNumber].name : emptyPhrase;
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 0f);
        _tweener = _text.DOFade(OpacityValue, FadeDuration).OnUpdate(() =>
        {
            if (_isDisplayStop)
            {
                _tweener.Kill();
                _isDisplayStop = false;
            }
        }).OnComplete(() =>
        {
            DisplayCompleted?.Invoke();
            _tweener = null;
        });
    }

    public void CleanText()
    {
        _text.text = null;

        if (_tweener != null)
        {
            _isDisplayStop = true;
        }
    }
}
