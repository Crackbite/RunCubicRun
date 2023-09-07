using Lean.Localization;
using TMPro;
using UnityEngine;

public class LevelSetter : MonoBehaviour
{
    [SerializeField] private TMP_Text _level;
    [SerializeField] private LeanLocalizedTextMeshProUGUI _levelLocalizedText;
    [SerializeField] private DataRestorer _dataRestorer;
    [SerializeField] private GameObject _trainingHeaderPhrase;
    [SerializeField] private GameObject _levelHeaderPhrase;
    [SerializeField] private AuthRequestScreen _authRequestScreen;

    private bool _canUpdateLevel;
    private int _currentLevel;
    private string _header;

    private void OnEnable()
    {
        _dataRestorer.DataRestored += OnDataRestored;
        _levelLocalizedText.TranslationUpdated += OnLevelTranslationUpdated;
        _authRequestScreen.PlayerAuthorized += OnPlayerAuthorized;
    }

    private void Awake()
    {
        _header = _levelHeaderPhrase.name;
    }

    private void OnDisable()
    {
        _dataRestorer.DataRestored -= OnDataRestored;
        _levelLocalizedText.TranslationUpdated -= OnLevelTranslationUpdated;
        _authRequestScreen.PlayerAuthorized -= OnPlayerAuthorized;
    }

    private void OnLevelTranslationUpdated()
    {
        if (_canUpdateLevel)
        {
            _canUpdateLevel = false;
            UpdateLevelText(_currentLevel);
        }
    }

    private void OnDataRestored(PlayerData playerData)
    {
        if (_currentLevel != playerData.Level || _currentLevel == 0)
        {
            UpdateLevelText(playerData.Level);
            _canUpdateLevel = true;
        }
    }

    private void OnPlayerAuthorized()
    {
        _level.text = _header;
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
