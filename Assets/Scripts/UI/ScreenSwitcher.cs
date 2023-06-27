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
    [SerializeField] private GameStatusTracker _gameStatusTracker;

    private Screen _currentScreen;

    private void OnEnable()
    {
        _menuScreen.StartClicked += OnMenuStartClicked;
        _menuScreen.StoreClicked += OnMenuStoreClicked;
        _gameStatusTracker.GameEnded += OnGameEnded;
        _storeScreen.CloseClicked += OnStoreCloseClicked;
    }

    private void Start()
    {
        SetDefaultScreen();
    }

    private void OnDisable()
    {
        _menuScreen.StartClicked -= OnMenuStartClicked;
        _menuScreen.StoreClicked -= OnMenuStoreClicked;
        _gameStatusTracker.GameEnded -= OnGameEnded;
        _storeScreen.CloseClicked -= OnStoreCloseClicked;
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

    private void OnStoreCloseClicked()
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

    private void SetGameScreen()
    {
        SetScreen(_gameScreen);
    }

    private void SetMenuScreen()
    {
        SetScreen(_menuScreen);
    }

    private void SetScreen(Screen newScreen)
    {
        SwitchCurrentScreen(newScreen);
        _currentScreen.Enter();
    }

    private void SetSuccessScreen()
    {
        SetScreen(_successScreen);
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