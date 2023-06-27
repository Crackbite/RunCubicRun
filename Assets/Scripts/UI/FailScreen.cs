using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FailScreen : Screen
{
    [SerializeField] private Button _home;
    [SerializeField] private Button _restart;
    [SerializeField] private float _maxWindowDelay = 1.5f;
    [SerializeField] private DOTweenAnimation _windowAnimation;
    [SerializeField] private DOTweenAnimation _containerAnimation;

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

    public void Enter(GameResult gameResult)
    {
        switch (gameResult)
        {
            case GameResult.LoseWithBlocksEnded:
            case GameResult.LoseWithPortalSuckedIn:
                _windowAnimation.delay = 0f;
                break;
            case GameResult.LoseWithHit:
            default:
                _windowAnimation.delay = _maxWindowDelay;
                break;
        }

        _containerAnimation.delay = _windowAnimation.delay + _windowAnimation.duration;
        base.Enter();
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