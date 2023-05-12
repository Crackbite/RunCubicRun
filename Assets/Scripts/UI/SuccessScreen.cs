using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SuccessScreen : Screen
{
    [SerializeField] private Button _next;
    [SerializeField] private Button _restart;

    private void OnEnable()
    {
        _next.onClick.AddListener(OnNextClicked);
        _restart.onClick.AddListener(OnRestartClicked);
    }

    private void OnDisable()
    {
        _next.onClick.RemoveListener(OnNextClicked);
        _restart.onClick.RemoveListener(OnRestartClicked);
    }

    private void LoadScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    private void OnNextClicked()
    {
        ChunkStorage.Instance.Restart();
        LoadScene();
    }

    private void OnRestartClicked()
    {
        LoadScene();
    }
}