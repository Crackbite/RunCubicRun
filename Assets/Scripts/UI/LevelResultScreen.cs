using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelResultScreen : Screen
{
    [SerializeField] private Button _restart;
    [SerializeField] private TMP_Text _level;
    [SerializeField] private GameDataHandler _gameDataHandler;
    [SerializeField] private LeanLocalizedTextMeshProUGUI _levelLocalizedText;
    [SerializeField] private GameObject _trainingStagePhrase;

    private int _gameFirstLevel = 1;

    protected virtual void OnEnable()
    {
        _restart.onClick.AddListener(OnRestartClicked);
        _levelLocalizedText.TranslationUpdated += OnLevelTranslationUpdated;

        if (_gameDataHandler.Level >= _gameFirstLevel)
        {
            _level.text += _gameDataHandler.Level.ToString();
        }
        else
        {
            _levelLocalizedText.TranslationName = _trainingStagePhrase.name;
        }
    }

    protected virtual void OnDisable()
    {
        _restart.onClick.RemoveListener(OnRestartClicked);
        _levelLocalizedText.TranslationUpdated -= OnLevelTranslationUpdated;
    }

    protected void LoadScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
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

    private void OnRestartClicked()
    {
        LoadScene();
    }
}
