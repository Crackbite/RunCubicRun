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
    private string _levelHeader;
    private string _trainingHeader;

    public event Action Set;

    private void OnEnable()
    {
        _levelLocalizedText.TranslationUpdated += OnLevelTranslationUpdated;
    }

    private void Awake()
    {
        _levelHeader = _levelHeaderPhrase.name;
        _trainingHeader = _trainingHeaderPhrase.name;
    }

    private void OnDisable()
    {
        _levelLocalizedText.TranslationUpdated -= OnLevelTranslationUpdated;
    }

    public void SetLevel(PlayerData playerData)
    {
        if (_currentLevel != playerData.Level || _currentLevel == 0)
        {
            UpdateLevelText(playerData.Level);
            _canUpdateLevel = true;
        }

        Set?.Invoke();
    }

    private void OnLevelTranslationUpdated()
    {
        const int FirstLevel = 1;

        if (_canUpdateLevel && _currentLevel >= FirstLevel)
        {
            _level.text = $"{_level.text} {_currentLevel}";
        }
    }

    private void UpdateLevelText(int level)
    {
        const int FirstLevel = 1;

        if (level >= FirstLevel)
        {
            _levelLocalizedText.TranslationName = _levelHeader;
            _level.text = $"{_level.text} {level}";
            _currentLevel = level;
        }
        else
        {
            _levelLocalizedText.TranslationName = _trainingHeader;
        }
    }
}
