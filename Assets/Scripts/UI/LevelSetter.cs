using Lean.Localization;
using System;
using TMPro;
using UnityEngine;

public class LevelSetter : MonoBehaviour
{
    [SerializeField] private TMP_Text _level;
    [SerializeField] private LeanLocalizedTextMeshProUGUI _levelLocalizedText;
    [SerializeField] private GameObject _trainingHeaderPhrase;
    [SerializeField] private GameObject _levelHeaderPhrase;

    private bool _canUpdateLevel;
    private int _currentLevel;
    private string _header;

    public event Action Set;

    private void OnEnable()
    {
        _levelLocalizedText.TranslationUpdated += OnLevelTranslationUpdated;
    }

    private void Awake()
    {
        _header = _levelHeaderPhrase.name;
    }

    private void OnDisable()
    {
        _levelLocalizedText.TranslationUpdated -= OnLevelTranslationUpdated;
    }

    public void SetLevel(PlayerData playerData)
    {
        _level.text = _header;

        if (_currentLevel != playerData.Level || _currentLevel == 0)
        {
            UpdateLevelText(playerData.Level);
            _canUpdateLevel = true;
        }

        Set?.Invoke();
    }

    private void OnLevelTranslationUpdated()
    {
        if (_canUpdateLevel)
        {
            _canUpdateLevel = false;
            UpdateLevelText(_currentLevel);
        }
    }

    private void UpdateLevelText(int level)
    {
        const int TrainingValue = 0;

        if (level > TrainingValue)
        {
            _levelLocalizedText.TranslationName = _levelHeaderPhrase.name;
            _header = _level.text;
            _level.text = $"{_level.text} {level}";
            _currentLevel = level;
        }
        else
        {
            _levelLocalizedText.TranslationName = _trainingHeaderPhrase.name;
        }
    }
}
