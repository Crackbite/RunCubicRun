using UnityEngine;

public class PauseEventSystem : MonoBehaviour
{
    [SerializeField] private PauseSystem _pauseSystem;
    [SerializeField] private SettingsScreen _settingsScreen;
    [SerializeField] private SDK _sdk;
    [SerializeField] private GameStatusTracker _gameStatusTracker;

    private bool _canUnpause = true;
    private bool _gameStarted;

    private void OnEnable()
    {
        _settingsScreen.Showed += OnSettingsScreenShowed;
        _settingsScreen.Hidden += OnSettingsScreenHidden;
        _sdk.AdOpened += OnAdOpened;
        _sdk.AdClosed += OnAdClosed;
        _gameStatusTracker.GameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        _settingsScreen.Showed -= OnSettingsScreenShowed;
        _settingsScreen.Hidden -= OnSettingsScreenHidden;
        _sdk.AdOpened -= OnAdOpened;
        _sdk.AdClosed -= OnAdClosed;
        _gameStatusTracker.GameStarted -= OnGameStarted;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            if (_canUnpause)
            {
                _pauseSystem.FocusGame();
            }
        }
        else
        {
            _pauseSystem.UnfocusGame();
        }
    }

    private void OnSettingsScreenHidden()
    {
        if (_gameStarted)
        {
            _canUnpause = true;
            _pauseSystem.FocusGame();
        }
    }

    private void OnSettingsScreenShowed()
    {
        if (_gameStarted)
        {
            _canUnpause = false;
            _pauseSystem.UnfocusGame();
        }
    }

    private void OnAdOpened()
    {
        _canUnpause = false;
        _pauseSystem.PauseGame();
    }

    private void OnAdClosed()
    {
        _canUnpause = true;
        _pauseSystem.UnpauseGame();
    }

    private void OnGameStarted()
    {
        _gameStarted = true;
    }
}
