using Lean.Localization;
using TMPro;
using UnityEngine;

public class MainScreen : Screen
{
    [SerializeField] private TMP_Text _level;
    [SerializeField] private LeanLocalizedTextMeshProUGUI _levelLocalizedText;
    [SerializeField] private GameDataHandler _gameDataHandler;
    [SerializeField] private bool _isTraining;

    private string _currentLevel = "";
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
            UpdateLevelText();
        }
    }

    private void OnDataRestored()
    {
        if (_isTraining == false)
        {
            UpdateLevelText();
            _canUpdateLevel = true;
        }
    }

    private void UpdateLevelText()
    {
        _currentLevel = _gameDataHandler.Level.ToString();
        _level.text = $"{_level.text} {_currentLevel}";
    }
}