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

    const float FadeDuration = 1.0f;

    public event Action DisplayCompleted;

    public int PhrasesAmount { get; private set; }

    private void Start()
    {
        PhrasesAmount = _phrases.Count;
    }

    public void Display(int _nextPhraseNumber)
    {
        const float OpacityValue = 1.0f;
        string emptyPhrase = "";

        _localizedText.TranslationName = _phrases[_nextPhraseNumber] != null ? _phrases[_nextPhraseNumber].name : emptyPhrase;
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 0f);
        _text.DOFade(OpacityValue, FadeDuration)
        .OnComplete(() =>
        {
            DisplayCompleted?.Invoke();
        });
    }

    public void CleanText()
    {
        const float OpacityValue = 0f;

        _text.DOFade(OpacityValue, FadeDuration);
    }
}
