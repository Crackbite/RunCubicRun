using UnityEngine;

public class PauseEventSystem : MonoBehaviour
{
    [SerializeField] private PauseSystem _pauseSystem;
    [SerializeField] private SettingsScreen _settingsScreen;
    [SerializeField] private SDK _sdk;

    private void OnEnable()
    {
        _settingsScreen.Showed += OnSettingsShowed;
        _settingsScreen.Hidden += OnSettingsHidden;
        _sdk.AdOpened += OnAdOpened;
        _sdk.AdClosed += OnAdClosed;
    }

    private void OnDisable()
    {
        _settingsScreen.Showed -= OnSettingsShowed;
        _settingsScreen.Hidden -= OnSettingsHidden;
        _sdk.AdOpened -= OnAdOpened;
        _sdk.AdClosed -= OnAdClosed;
    }

    private void OnSettingsHidden()
    {
       _pauseSystem.UnpauseGame();
    }

    private void OnSettingsShowed()
    {
        _pauseSystem.PauseGame();
    }

    private void OnAdOpened()
    {
        _pauseSystem.PauseGame();
    }

    private void OnAdClosed()
    {
        _pauseSystem.UnpauseGame();
    }
}
