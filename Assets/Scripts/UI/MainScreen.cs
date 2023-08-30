using Lean.Localization;
using TMPro;
using UnityEngine;

public class MainScreen : Screen
{
    [SerializeField] private TMP_Text _level;
    [SerializeField] private LeanLocalizedTextMeshProUGUI _levelLocalizedText;
    [SerializeField] private DataRestorer _dataRestorer;
    [SerializeField] private GameObject _trainingHeaderPhrase;
    [SerializeField] private GameObject _levelHeaderPhrase;

    private bool _canUpdateLevel;

    private void OnEnable()
    {
        _dataRestorer.DataRestored += OnDataRestored;
        _levelLocalizedText.TranslationUpdated += OnLevelTranslationUpdated;
    }

    private void OnDisable()
    {
        _dataRestorer.DataRestored -= OnDataRestored;
        _levelLocalizedText.TranslationUpdated -= OnLevelTranslationUpdated;
    }

    private void OnLevelTranslationUpdated()
    {
        if (_canUpdateLevel)
        {
            UpdateLevelText(_dataRestorer.Level);
        }
    }

    private void OnDataRestored()
    {
        UpdateLevelText(_dataRestorer.Level);
        _canUpdateLevel = true;
    }

    private void UpdateLevelText(int currentLevel)
    {
        const int TrainingValue = 0;

        _levelLocalizedText.TranslationName = currentLevel > TrainingValue ? _levelHeaderPhrase.name : _trainingHeaderPhrase.name;

        if (currentLevel > TrainingValue)
        {
            _levelLocalizedText.TranslationName = _levelHeaderPhrase.name;
            _level.text = $"{_level.text} {currentLevel}";
        }
        else
        {
            _levelLocalizedText.TranslationName = _trainingHeaderPhrase.name;
        }
    }
}