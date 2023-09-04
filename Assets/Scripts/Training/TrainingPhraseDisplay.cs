﻿using Lean.Localization;
using System;
using UnityEngine;

public class TrainingPhraseDisplay : MonoBehaviour
{
    [SerializeField] private LeanLocalizedTextMeshProUGUI _localizedText;
    [SerializeField] private TrainingStageHolder _trainingStageHolder;
    [SerializeField] private LeanLocalization _localization;
    [SerializeField] private ContinuePromptText _continuePromptText;
    [SerializeField] private PauseSystem _pauseSystem;
    [SerializeField] private DataRestorer _dataRestorer;

    private TrainingStageInfo _currentStageInfo;

    public event Action Completed;

    public int PhrasesAmount { get; private set; }

    private void OnEnable()
    {
        _pauseSystem.TimeChanged += OnTimeScaled;
        _dataRestorer.DataRestored += OnDataRestored;
    }

    private void OnDisable()
    {
        _pauseSystem.TimeChanged -= OnTimeScaled;
        _dataRestorer.DataRestored -= OnDataRestored;
    }

    public void Display(int _nextPhraseNumber)
    {
        string emptyPhrase = "";

        _localizedText.TranslationName = _currentStageInfo.TrainingPhrases[_nextPhraseNumber] != null
            ? _currentStageInfo.TrainingPhrases[_nextPhraseNumber].name : emptyPhrase;

        _pauseSystem.SlowDownTime();
    }

    public void End()
    {
        _pauseSystem.AccelerateTime();
    }

    public void EndImmediately()
    {
        _pauseSystem.UnpauseGame();
    }

    public bool CanEndTrainingPause()
    {
        return _pauseSystem.CanEndTrainingPause;
    }

    private void OnDataRestored(PlayerData playerData)
    {
        int stage = playerData.TrainingStage;

        if (_trainingStageHolder.TryGetStageInfo(stage, out TrainingStageInfo currentStageInfo))
        {
            _currentStageInfo = currentStageInfo;
            PhrasesAmount = currentStageInfo.TrainingPhrases.Count;
        }
    }

    private void OnTimeScaled()
    {
        _continuePromptText.enabled = _continuePromptText.enabled == false;
        Completed?.Invoke();
    }
}
