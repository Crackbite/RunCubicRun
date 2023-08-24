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
    [SerializeField] private GameDataHandler _gameDataHandler;

    private Screen _currentScreen;

    public event Action GameScreenSet;

    private void OnEnable()
    {
        _gameDataHandler.DataRestored += OnDataRestored;
        _menuScreen.StartClicked += OnMenuStartClicked;
        _menuScreen.StoreClicked += OnMenuStoreClicked;
        _menuScreen.LeaderboardClicked += OnLeaderboardClicked;
        _gameStatusTracker.GameEnded += OnGameEnded;
        _storeScreen.CloseClicked += OnStoreCloseClicked;
        _leaderboardScreen.CloseClicked += OnLeaderboardCloseClicked;
    }

    private void OnDisable()
    {
        _gameDataHandler.DataRestored -= OnDataRestored;
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

        if ( PlayerAccount.IsAuthorized)
        {
            Debug.Log("AUTHORIZED");
            SetLeaderboardScreen();
            return;
        }
       
        SetAuthRequestScreen();
    }

    private void OnDataRestored()
    {
        if (_gameDataHandler.IsLevelRestarting)
        {
            SetGameScreen();
        }
        else
        {
            SetDefaultScreen();
        }
    }

    private void OnStoreCloseClicked()
    {
        SetScreen(_menuScreen);
    }

    private void OnLeaderboardCloseClicked()
    {
        SetScreen(_menuScreen);
    }

    private void SetDefaultScreen()
    {
        _mainScreen.Enter();
        SetMenuScreen();
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

    private void SetAuthRequestScreen()
    {
        SetScreen(_authRequestScreen);
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
        SwitchCurrentScreen(newScreen);
        _currentScreen.Enter();
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