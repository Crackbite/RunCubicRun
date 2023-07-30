using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private const string LevelKey = nameof(LevelKey);
    private const string TrainingStageKey = nameof(TrainingStageKey);

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static private void OnRuntimeMethodLoad()
    {
        int level = PlayerPrefs.GetInt(LevelKey, 0);

        if (level == 0)
        {
            int trainingStage = PlayerPrefs.GetInt(TrainingStageKey, 0);
            SceneManager.LoadScene(trainingStage);
        }
        else
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            SceneManager.LoadScene(sceneCount - 1);
        }
    }
}
