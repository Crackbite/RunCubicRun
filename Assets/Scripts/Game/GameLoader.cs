using IJunior.TypedScenes;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    [SerializeField] private LevelResultScreen _failScreen;
    [SerializeField] private LevelResultScreen _successScreen;
    [SerializeField] private LevelLoadConfig _levelLoadConfig;

    private void OnEnable()
    {
        _failScreen.SceneLoading += OnSceneLoading;
        _successScreen.SceneLoading += OnSceneLoading;
    }

    private void OnDisable()
    {
        _failScreen.SceneLoading -= OnSceneLoading;
        _successScreen.SceneLoading -= OnSceneLoading;
    }

    private void OnSceneLoading(bool isStartWithoutMenu)
    {
        _failScreen.SceneLoading -= OnSceneLoading;
        _successScreen.SceneLoading -= OnSceneLoading;
        _levelLoadConfig.IsStartWithoutMenu = isStartWithoutMenu;
        MainScene.Load(_levelLoadConfig);
    }
}
