using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    [SerializeField] private SettingsScreen _settingsScreen;

    private void OnEnable()
    {
        _settingsScreen.Showed += OnSettingsShowed;
        _settingsScreen.Hidden += OnSettingsHidden;
    }

    private void OnDisable()
    {
        _settingsScreen.Showed -= OnSettingsShowed;
        _settingsScreen.Hidden -= OnSettingsHidden;
    }

    private void OnSettingsHidden()
    {
        ResumeGame();
    }

    private void OnSettingsShowed()
    {
        PauseGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
    }
}