using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SuccessScreen : Screen
{
    [SerializeField] private Button _restart;

    private void OnEnable()
    {
        _restart.onClick.AddListener(OnRestartClicked);
    }

    private void OnDisable()
    {
        _restart.onClick.RemoveListener(OnRestartClicked);
    }

    private void OnRestartClicked()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}