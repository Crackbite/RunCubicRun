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
    [SerializeField] private SDK _sdk;

    private int _gameFirstLevel = 1;
    private bool _canUpdateLevel;

    protected bool IsTraining;

    public event Action<bool> SceneLoading;

    protected virtual void OnEnable()
    {
        _home.onClick.AddListener(OnHomeClicked);
        _sdk.AdClosed += OnAdClosed;
        _levelLocalizedText.TranslationUpdated += OnLevelTranslationUpdated;

        SetLevel();
        _canUpdateLevel = true;
    }

    protected virtual void OnDisable()
    {
        _home.onClick.RemoveListener(OnHomeClicked);
        _sdk.AdClosed -= OnAdClosed;
        _levelLocalizedText.TranslationUpdated += OnLevelTranslationUpdated;
    }

    protected abstract void OnHomeClicked();

    protected abstract void OnAdClosed();

    protected void LoadScene(bool isStartWithoutMenu = false)
    {
        SceneLoading?.Invoke(isStartWithoutMenu);
    }

    private void OnLevelTranslationUpdated()
    {
        if (_canUpdateLevel)
        {
            SetLevel();
        }
    }

    private void SetLevel()
    {
        int currentStage = _dataRestorer.CurrentTrainingStage;
        int trainingStageAmount = _dataRestorer.TrainingStageAmount;
        int currentLevel = _dataRestorer.CurrentLevel;

        if (currentLevel >= _gameFirstLevel)
        {
            _level.text = $"{_level.text} {currentLevel}";
        }
        else
        {
            IsTraining = true;
            _levelLocalizedText.TranslationName = _trainingStagePhrase.name;
            _level.text = $"{_level.text} {currentStage}/{trainingStageAmount}";
        }
    }
}
