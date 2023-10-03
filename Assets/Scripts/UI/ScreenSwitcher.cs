using Agava.YandexGames;
using System;
using UnityEngine;

public class ScreenSwitcher : MonoBehaviour
{
    [SerializeField] private MainScreen _mainScreen;
    [SerializeField] private MenuScreen _menuScreen;
    [SerializeField] private GameScreen _gameScreen;
    [SerializeField] private SuccessScreen _successScreen;
    [SerializeField] private FailScreen _failScreen;
    [SerializeField] private StoreScreen _storeScreen;
    [SerializeField] private LeaderboardScreen _leaderboardScreen;
    [SerializeField] private AuthRequestScreen _authRequestScreen;
    [SerializeField] private GameStatusTracker _gameStatusTracker;
    [SerializeField] private LeaderboardLoader _leaderboardLoader;

    private Screen _currentScreen;
    private bool _isMainScreenShowed;

    public event Action GameScreenSet;

    private void OnEnable()
    {
        _mainScreen.Showed += OnMainScreenShowed;
        _menuScreen.StartClicked += OnMenuStartClicked;
        _menuScreen.StoreClicked += OnMenuStoreClicked;
        _menuScreen.LeaderboardClicked += OnLeaderboardClicked;
        _gameStatusTracker.GameEnded += OnGameEnded;
        _storeScreen.CloseClicked += OnStoreCloseClicked;
        _leaderboardScreen.CloseClicked += OnLeaderboardCloseClicked;
    }

    private void Start()
    {
        if (_mainScreen.enabled && _isMainScreenShowed == false)
        {
            OnMainScreenShowed();
        }
    }

    private void OnDisable()
    {
        _mainScreen.Showed -= OnMainScreenShowed;
        _menuScreen.StartClicked -= OnMenuStartClicked;
        _menuScreen.StoreClicked -= OnMenuStoreClicked;
        _menuScreen.LeaderboardClicked -= OnLeaderboardClicked;
        _gameStatusTracker.GameEnded -= OnGameEnded;
        _storeScreen.CloseClicked -= OnStoreCloseClicked;
        _leaderboardScreen.CloseClicked -= OnLeaderboardCloseClicked;
    }

    private void OnGameEnded(GameResult gameResult)
    {
        if (gameResult == GameResult.Win)
        {
            SetSuccessScreen();
        }
        else
        {
            SetFailScreen(gameResult);
        }

        _mainScreen.Exit();
    }

    private void OnMainScreenShowed()
    {
        _isMainScreenShowed = true;

        if (_gameStatusTracker.IsStartWithoutMenu)
        {
            SetGameScreen();
        }
        else
        {
            SetMenuScreen();
        }
    }

    private void OnMenuStartClicked()
    {
        SetGameScreen();
    }

    private void OnMenuStoreClicked()
    {
        _mainScreen.Exit();
        SetStoreScreen();
    }

    private void OnLeaderboardClicked()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        _mainScreen.Exit();
        SetLeaderboardScreen();
        return;
#endif

        if (PlayerAccount.IsAuthorized)
        {
            if (PlayerAccount.HasPersonalProfileDataPermission == false)
            {
                PlayerAccount.RequestPersonalProfileDataPermission();
            }

            _mainScreen.Exit();
            SetLeaderboardScreen();
        }
    }

    private void OnStoreCloseClicked()
    {
        _storeScreen.Exit();
    }

    private void OnLeaderboardCloseClicked()
    {
        _leaderboardScreen.Exit();
    }

    private void SetFailScreen(GameResult gameResult)
    {
        SwitchCurrentScreen(_failScreen);
        _failScreen.Enter(gameResult);
    }

    private void SetStoreScreen()
    {
        SetScreen(_storeScreen);
    }

    private void SetLeaderboardScreen()
    {
        SetScreen(_leaderboardScreen);
    }

    private void SetGameScreen()
    {
        SetScreen(_gameScreen);
        GameScreenSet?.Invoke();
    }

    private void SetMenuScreen()
    {
        SetScreen(_menuScreen);
    }

    private void SetSuccessScreen()
    {
        SetScreen(_successScreen);
    }

    private void SetScreen(Screen newScreen)
    {
        if (_currentScreen != newScreen)
        {
            SwitchCurrentScreen(newScreen);
            _currentScreen.Enter();
        }
    }

    private void SwitchCurrentScreen(Screen newScreen)
    {
        if (_currentScreen != null)
        {
            _currentScreen.Exit();
        }

        _currentScreen = newScreen;
    }
}