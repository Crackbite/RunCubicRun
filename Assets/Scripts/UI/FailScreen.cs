using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FailScreen : Screen
{
    [SerializeField] private Button _home;
    [SerializeField] private Button _restart;

    private void OnEnable()
    {
        _home.onClick.AddListener(OnHomeClicked);
        _restart.onClick.AddListener(OnRestartClicked);
    }

    private void OnDisable()
    {
        _home.onClick.RemoveListener(OnHomeClicked);
        _restart.onClick.RemoveListener(OnRestartClicked);
    }

    private void LoadScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    private void OnHomeClicked()
    {
        ChunkStorage.Instance.Restart();
        LoadScene();
    }

    private void OnRestartClicked()
    {
        LoadScene();
    }
}