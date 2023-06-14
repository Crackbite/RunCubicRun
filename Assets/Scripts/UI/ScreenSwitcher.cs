using UnityEngine;

public class ScreenSwitcher : MonoBehaviour
{
    [SerializeField] private MainScreen _mainScreen;
    [SerializeField] private MenuScreen _menuScreen;
    [SerializeField] private GameScreen _gameScreen;
    [SerializeField] private SuccessScreen _successScreen;
    [SerializeField] private FailScreen _failScreen;
    [SerializeField] private GameStatusTracker _gameStatusTracker;

    private Screen _currentScreen;

    private void OnEnable()
    {
        _menuScreen.StartClicked += OnMenuStartClicked;
        _gameStatusTracker.GameEnded += OnGameEnded;
    }

    private void Start()
    {
        SetDefaultScreen();
    }

    private void OnDisable()
    {
        _menuScreen.StartClicked -= OnMenuStartClicked;
        _gameStatusTracker.GameEnded -= OnGameEnded;
    }

    private void OnGameEnded(GameResult gameResult)
    {
        if (gameResult == GameResult.Win)
        {
            SetSuccessScreen();
        }
        else
        {
            SetFailScreen();
        }

        _mainScreen.Exit();
    }

    private void OnMenuStartClicked()
    {
        SetGameScreen();
    }

    private void SetDefaultScreen()
    {
        _mainScreen.Enter();
        SetMenuScreen();
    }

    private void SetFailScreen()
    {
        SetScreen(_failScreen);
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
        if (_currentScreen != null)
        {
            _currentScreen.Exit();
        }

        _currentScreen = newScreen;
        _currentScreen.Enter();
    }

    private void SetSuccessScreen()
    {
        SetScreen(_successScreen);
    }
}