using Lean.Localization;
using TMPro;
using UnityEngine;

public class MainScreen : Screen
{
    [SerializeField] private TMP_Text _level;
    [SerializeField] private LeanLocalizedTextMeshProUGUI _levelLocalizedText;
    [SerializeField] private GameDataHandler _gameDataHandler;
    [SerializeField] private GameObject _trainingHeaderPhrase;
    [SerializeField] private GameObject _levelHeaderPhrase;

    private bool _canUpdateLevel;

    private void OnEnable()
    {
        _gameDataHandler.DataRestored += OnDataRestored;
        _levelLocalizedText.TranslationUpdated += OnLevelUpdated;
    }

    private void OnDisable()
    {
        _gameDataHandler.DataRestored -= OnDataRestored;
        _levelLocalizedText.TranslationUpdated -= OnLevelUpdated;
    }

    private void OnLevelUpdated()
    {
        if (_canUpdateLevel)
        {
            UpdateLevelText(_gameDataHandler.Level);
        }
    }

    private void OnDataRestored()
    {
        UpdateLevelText(_gameDataHandler.Level);
        _canUpdateLevel = true;
    }

    private void UpdateLevelText(int currentLevel)
    {
        const int TrainingValue = 0;

        _levelLocalizedText.TranslationName = currentLevel > TrainingValue ? _levelHeaderPhrase.name : _trainingHeaderPhrase.name;

        if (currentLevel > TrainingValue)
        {
            _level.text = $"{_level.text} {currentLevel}";
        }
    }
}