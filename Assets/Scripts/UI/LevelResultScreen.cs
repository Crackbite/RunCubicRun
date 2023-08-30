using Lean.Localization;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class LevelResultScreen : Screen
{
    [SerializeField] private Button _home;
    [SerializeField] private TMP_Text _level;
    [SerializeField] private DataRestorer _dataRestorer;
    [SerializeField] private LeanLocalizedTextMeshProUGUI _levelLocalizedText;
    [SerializeField] private GameObject _trainingStagePhrase;

    private int _gameFirstLevel = 1;

    protected bool IsTraining;
    protected int CurrentLevel;

    public event Action<bool> SceneLoading;

    protected virtual void OnEnable()
    {
        _home.onClick.AddListener(OnHomeClicked);
        _levelLocalizedText.TranslationUpdated += OnLevelTranslationUpdated;

        if (_dataRestorer.Level >= _gameFirstLevel)
        {
            _level.text += _dataRestorer.Level.ToString();
            CurrentLevel = _dataRestorer.Level;
        }
        else
        {
            IsTraining = true;
            _levelLocalizedText.TranslationName = _trainingStagePhrase.name;
        }
    }

    protected virtual void OnDisable()
    {
        _home.onClick.RemoveListener(OnHomeClicked);
        _levelLocalizedText.TranslationUpdated -= OnLevelTranslationUpdated;
    }

    protected abstract void OnHomeClicked();

    protected void LoadScene(bool isStartWithoutMenu = false)
    {
        SceneLoading?.Invoke(isStartWithoutMenu);
    }

    private void UpdateTrainingStageText()
    {
        int currentStage = _dataRestorer.TrainingStageNumber;
        int trainingStageAmount = _dataRestorer.TrainingStageAmount;

        _levelLocalizedText.TranslationName = _trainingStagePhrase.name;
        _level.text = $"{_level.text} {currentStage}/{trainingStageAmount}";
    }

    private void OnLevelTranslationUpdated()
    {
        if (_dataRestorer.Level >= _gameFirstLevel)
        {
            return;
        }

        UpdateTrainingStageText();
    }
}
