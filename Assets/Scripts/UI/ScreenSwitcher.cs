using Agava.YandexGames;
using System;
using System.Collections;
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
    [SerializeField] private DataRestorer _dataRestorer;
    [SerializeField] private LeaderboardLoader _leaderboardLoader;

    private Screen _currentScreen;

    public event Action GameScreenSet;

    private void OnEnable()
    {
        _dataRestorer.DataRestored += OnDataRestored;
        _mainScreen.Showed += OnMainScreenShowed;
        _menuScreen.StartClicked += OnMenuStartClicked;
        _menuScreen.StoreClicked += OnMenuStoreClicked;
        _menuScreen.LeaderboardClicked += OnLeaderboardClicked;
        _gameStatusTracker.GameEnded += OnGameEnded;
        _storeScreen.CloseClicked += OnCloseClicked;
        _leaderboardScreen.CloseClicked += OnCloseClicked;
        _authRequestScreen.CloseClicked += OnAuthRequestCloseClicked;
    }

    private void OnDisable()
    {
        _dataRestorer.DataRestored -= OnDataRestored;
        _mainScreen.Showed -= OnMainScreenShowed;
        _menuScreen.StartClicked -= OnMenuStartClicked;
        _menuScreen.StoreClicked -= OnMenuStoreClicked;
        _menuScreen.LeaderboardClicked -= OnLeaderboardClicked;
        _gameStatusTracker.GameEnded -= OnGameEnded;
        _storeScreen.CloseClicked -= OnCloseClicked;
        _leaderboardScreen.CloseClicked -= OnCloseClicked;
        _authRequestScreen.CloseClicked -= OnAuthRequestCloseClicked;
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

    private void OnDataRestored(PlayerData playerData)
    {
        if (_mainScreen.IsHidden)
        {
            _mainScreen.Enter();
        }
        else
        {
            StartCoroutine(WaitForMainScreenToHide());
        }
    }

    private void OnMainScreenShowed()
    {
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
        SetStoreScreen();
    }

    private void OnLeaderboardClicked()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        SetLeaderboardScreen();
        return;
#endif

        if (PlayerAccount.IsAuthorized)
        {
            SetLeaderboardScreen();
            return;
        }
    }

    private void OnCloseClicked()
    {
        SetScreen(_menuScreen);
    }

    private void OnAuthRequestCloseClicked()
    {
        _mainScreen.Exit();
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

    private IEnumerator WaitForMainScreenToHide()
    {
        while (_mainScreen.IsHidden == false)
        {
            yield return null;
        }

        _mainScreen.Enter();
    }
}