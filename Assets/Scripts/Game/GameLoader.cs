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

    private void Start()
    {
        _levelLoadConfig.ResetConfig();
    }

    private void OnDisable()
    {
        _failScreen.SceneLoading -= OnSceneLoading;
        _successScreen.SceneLoading -= OnSceneLoading;
    }

    private void OnSceneLoading(bool isStartWithoutMenu)
    {
        if (isStartWithoutMenu)
        {
            _levelLoadConfig.SetStartWithoutMenu();
        }

        MainScene.Load(_levelLoadConfig);
    }
}
