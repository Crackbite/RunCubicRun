using IJunior.TypedScenes;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    [SerializeField] private LevelResultScreen _failScreen;
    [SerializeField] private LevelResultScreen _successScreen;
    [SerializeField] private AuthRequestScreen _authRequestScreen;
    [SerializeField] private LevelLoadConfig _levelLoadConfig;

    private void OnEnable()
    {
        _failScreen.SceneLoading += OnSceneLoading;
        _successScreen.SceneLoading += OnSceneLoading;
        _authRequestScreen.PlayerAuthorized += OnPlayerAuthorized;
    }

    private void Start()
    {
        _levelLoadConfig.ResetConfig();
    }

    private void OnDisable()
    {
        _failScreen.SceneLoading -= OnSceneLoading;
        _successScreen.SceneLoading -= OnSceneLoading;
        _authRequestScreen.PlayerAuthorized -= OnPlayerAuthorized;
    }

    private void OnSceneLoading(bool isStartWithoutMenu)
    {
        if (isStartWithoutMenu)
        {
            _levelLoadConfig.SetStartWithoutMenu();
        }

        MainScene.Load(_levelLoadConfig);
    }

    private void OnPlayerAuthorized()
    {
        _levelLoadConfig.SetPlayerHasAuth();
        MainScene.Load(_levelLoadConfig);
    }
}
