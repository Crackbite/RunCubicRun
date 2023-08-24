using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class LevelResultScreen : Screen
{
    [SerializeField] private Button _home;
    [SerializeField] private TMP_Text _level;
    [SerializeField] private GameDataHandler _gameDataHandler;
    [SerializeField] private LeanLocalizedTextMeshProUGUI _levelLocalizedText;
    [SerializeField] private GameObject _trainingStagePhrase;

    private int _gameFirstLevel = 1;
    private bool _isLevelRestarting = true;

    protected bool IsTraining;
    protected int CurrentLevel;

    protected virtual void OnEnable()
    {
        _home.onClick.AddListener(OnHomeClicked);
        _levelLocalizedText.TranslationUpdated += OnLevelTranslationUpdated;

        if (_gameDataHandler.Level >= _gameFirstLevel)
        {
            _level.text += _gameDataHandler.Level.ToString();
            CurrentLevel = _gameDataHandler.Level;
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

    protected void LoadScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);

        if (_isLevelRestarting)
        {
            RestartLevel();
        }
    }

    protected abstract void RestartLevel();

    protected virtual void OnHomeClicked()
    {
        _isLevelRestarting = false;

#if !UNITY_WEBGL || UNITY_EDITOR
        LoadScene();
        return;
#endif
    }

    private void UpdateTrainingStageText()
    {
        int currentStage = _gameDataHandler.TrainingStageNumber;
        int trainingStageAmount = _gameDataHandler.TrainingStageAmount;

        _levelLocalizedText.TranslationName = _trainingStagePhrase.name;
        _level.text = $"{_level.text} {currentStage}/{trainingStageAmount}";
    }

    private void OnLevelTranslationUpdated()
    {
        if (_gameDataHandler.Level >= _gameFirstLevel)
        {
            return;
        }

        UpdateTrainingStageText();
    }
}
