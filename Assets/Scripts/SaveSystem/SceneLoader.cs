using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private const string LevelKey = nameof(LevelKey);
    private const string TrainingStageKey = nameof(TrainingStageKey);

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static private void OnRuntimeMethodLoad()
    {
        const int DefaultValue = 0;

        int level = PlayerPrefs.GetInt(LevelKey, DefaultValue);

        if (level == 0)
        {
            int trainingStage = PlayerPrefs.GetInt(TrainingStageKey, DefaultValue);
            SceneManager.LoadScene(trainingStage);
        }
        else
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            SceneManager.LoadScene(sceneCount - 1);
        }
    }
}
