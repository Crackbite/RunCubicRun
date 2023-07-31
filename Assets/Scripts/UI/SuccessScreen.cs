using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SuccessScreen : Screen
{
    [SerializeField] private Button _next;
    [SerializeField] private Button _restart;
    [SerializeField] private TMP_Text _level;
    [SerializeField] private GameDataHandler _gameDataHandler;

    private const int TrainingStageAmount = 4;

    private void OnEnable()
    {
        _next.onClick.AddListener(OnNextClicked);
        _restart.onClick.AddListener(OnRestartClicked);
        _gameDataHandler.DataRestored += OnDataRestored;
    }

    private void OnDisable()
    {
        _next.onClick.RemoveListener(OnNextClicked);
        _restart.onClick.RemoveListener(OnRestartClicked);
        _gameDataHandler.DataRestored -= OnDataRestored;
    }

    private void LoadScene()
    {
        int sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;

        if (sceneBuildIndex < TrainingStageAmount)
        {
            SceneManager.LoadScene(++sceneBuildIndex);
        }
        else
        {
            SceneManager.LoadScene(sceneBuildIndex);
        }
    }

    private void OnDataRestored()
    {
        _level.text += (_gameDataHandler.Level - 1).ToString();
    }

    private void OnNextClicked()
    {
        if (ChunkStorage.Instance != null)
        {
            ChunkStorage.Instance.Restart();
        }

        LoadScene();
    }

    private void OnRestartClicked()
    {
        LoadScene();
    }
}